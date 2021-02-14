﻿// This file is part of Graph3DVisualizer.
// Copyright © Gershuk Vladislav 2021.
//
// Graph3DVisualizer is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Graph3DVisualizer is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Graph3DVisualizer.  If not, see <https://www.gnu.org/licenses/>.

using System;

using Grpah3DVisualizer.Customizable;

using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEngine.Physics;

namespace Grpah3DVisualizer.PlayerInputControls
{
    public readonly struct ToolConfig
    {
        public ToolParams[] ToolParams { get; }
        public Type ToolType { get; }

        public ToolConfig (Type toolType, params ToolParams[] toolParams)
        {
            if (!toolType.IsSubclassOf(typeof(PlayerTool)))
                throw new Exception($"{toolType} is not subclass of PlayerTool");
            ToolType = toolType ?? throw new ArgumentNullException(nameof(toolType));
            ToolParams = toolParams;
        }
    }

    public abstract class PlayerTool : MonoBehaviour
    {
        protected RaycastHit RayCast (float range)
        {
            Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit, range);
            return hit;
        }

        public abstract void RegisterEvents (IInputActionCollection inputActions);
    }

    public abstract class ToolParams : CustomizableParameter
    { }
}