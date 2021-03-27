﻿// This file is part of Grpah3DVisualizer.
// Copyright © Gershuk Vladislav 2020.
//
// Grpah3DVisualizer is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Grpah3DVisualizer is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Grpah3DVisualizer.  If not, see <https://www.gnu.org/licenses/>.

using System.Linq;

using UnityEngine;

public class GraphicSettings : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Dropdown _dropdown;

    private void Start ()
    {
        _dropdown.ClearOptions();
        _dropdown.AddOptions(QualitySettings.names.ToList());
        _dropdown.value = QualitySettings.GetQualityLevel();
    }

    public void SetQuality ()
    {
        QualitySettings.SetQualityLevel(_dropdown.value);
    }
}