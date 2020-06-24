using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Clement.Persistence.Localization
{
    public static class LocalizationUtilities
    {
        public static T ConvertAs<T>(this LocalizedItem item)
        {
            //return (T)item.value;
            //Debug.Log($"Type d'entrée : {typeof(T).GetFriendlyName()}");


            TypeCode t = Type.GetTypeCode(typeof(T));
            LocalizedValue v = item.value;
            switch (t)
            {
                case TypeCode.Int32:
                    return (T)(object)Convert.ToInt32(v.@int);

                case TypeCode.Decimal:
                    return (T)(object)Convert.ToDecimal(v.@float);

                case TypeCode.Boolean:
                    return (T)(object)Convert.ToBoolean(v.@bool);

                case TypeCode.String:
                    return (T)(object)Convert.ToString(v.@string);

                case TypeCode.Object:
                    //Debug.Log(v.@object);
                    //Debug.Log($"Type de cet objet : {v.@object.GetType().GetFriendlyName()}");
                    try
                    {
                        return (T)(object)v.@object;
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"Erreur : {e.Message} \n Vous tentez de récupérer un objet de type {typeof(T)} alors que l'objet est de type {v.@object.GetType().GetFriendlyName()}.");
                        return default;
                    }
                default:
                    return default;
            }

        }

        public static LocalizedValue ConvertAsData(this object value)
        {
            TypeCode t = Type.GetTypeCode(value.GetType());
            LocalizedValue v = new LocalizedValue();


            switch (t)
            {
                case TypeCode.Int32:
                    v.@int = (int)value;
                    break;

                case TypeCode.Decimal:
                    v.@float = (float)value;
                    break;

                case TypeCode.Boolean:
                    v.@bool = (bool)value;
                    break;

                case TypeCode.String:
                    v.@string = (string)value;
                    break;

                case TypeCode.Object:
                    v.@object = (UnityEngine.Object)value;
                    break;

                default:
                    break;
            }

            return v;
        }
    }
}
