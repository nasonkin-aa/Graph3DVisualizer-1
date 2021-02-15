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
using System.Collections.Generic;
using System.Runtime.Serialization;

//ToDo need to refactor this module
namespace Graph3DVisualizer.Customizable
{
    public static class CustomizableExtension
    {
        public static AbstractCustomizableParameter CallDownloadParams (object customizable) =>
           (AbstractCustomizableParameter) customizable.GetType().GetMethod(nameof(ICustomizable<AbstractCustomizableParameter>.DownloadParams),
           new[] { (Attribute.GetCustomAttribute(customizable.GetType(), typeof(CustomizableGrandTypeAttribute), true) as CustomizableGrandTypeAttribute).Type })
           .Invoke(customizable, null);

        public static List<T> CallDownloadParams<T> (object customizable) where T : AbstractCustomizableParameter
        {
            var parameters = new List<T>();
            foreach (var interfaceType in customizable.GetType().GetInterfaces())
            {
                if (interfaceType.GetGenericTypeDefinition() == typeof(ICustomizable<>)
                    && (interfaceType.GetGenericArguments()[0].IsSubclassOf(typeof(T)) || interfaceType.GetGenericArguments()[0] == typeof(T)))
                {
                    parameters.Add((T) interfaceType.GetMethod(nameof(ICustomizable<AbstractCustomizableParameter>.DownloadParams)).Invoke(customizable, null));
                }
            }
            return parameters;
        }

        public static void CallSetUpParams (object customizable, object parameter) =>
            customizable.GetType().GetMethod(nameof(ICustomizable<AbstractCustomizableParameter>.SetupParams),
            new[] { (Attribute.GetCustomAttribute(customizable.GetType(), typeof(CustomizableGrandTypeAttribute), true) as CustomizableGrandTypeAttribute).Type })
            .Invoke(customizable, new[] { parameter });

        public static void CallSetUpParams (object customizable, object[] parameters)
        {
            foreach (var param in parameters)
            {
                var isFinded = false;

                foreach (var interfaceType in customizable.GetType().GetInterfaces())
                {
                    if (interfaceType.GetGenericTypeDefinition() == typeof(ICustomizable<>) && interfaceType.GetGenericArguments()[0] == param.GetType())
                    {
                        interfaceType.GetMethod(nameof(ICustomizable<AbstractCustomizableParameter>.SetupParams), interfaceType.GetGenericArguments()).Invoke(customizable, new[] { param });
                        isFinded = true;
                    }
                }

                if (!isFinded)
                    throw new Exception($"Customizable methods with parameter type {param.GetType()} not found");
            }
        }
    }

    public abstract class AbstractCustomizableParameter { };

    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    sealed public class CustomizableGrandTypeAttribute : Attribute
    {
        private Type _type;
        public Type Type { get => _type; set => _type = value.IsSubclassOf(typeof(AbstractCustomizableParameter)) ? value : throw new WrongTypeInCustomizableParameterException(); }
    }

    public class WrongTypeInCustomizableParameterException : Exception
    {
        protected WrongTypeInCustomizableParameterException (SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public WrongTypeInCustomizableParameterException ()
        {
        }

        public WrongTypeInCustomizableParameterException (string message) : base(message)
        {
        }

        public WrongTypeInCustomizableParameterException (string message, Exception innerException) : base(message, innerException)
        {
        }

        public WrongTypeInCustomizableParameterException (Type expectedType, Type receivedType) : base($"The type inherited from {expectedType.Name} was expected, and {receivedType.Name} was obtained.")
        {
        }
    }

    public interface ICustomizable<TParams> where TParams : AbstractCustomizableParameter
    {
        TParams DownloadParams ();

        void SetupParams (TParams parameters);
    }
}