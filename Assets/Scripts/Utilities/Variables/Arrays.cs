using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using static UnityEngine.Debug;

namespace Clement.Utilities.Arrays
{
    public static class Arrays
    {

        /// <summary>
        /// Renvoie une string affichant le contenu du tableau.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string content<T>(this T[] array, string variableName)
        {
            if (array == null)
            {
                return $"Le tableau \"{variableName}\" n'est pas initialisé ; impossible d'obtenir son contenu.\n";
            }

            //+1 pour ajouter le nom de la variable au début
            StringBuilder sb = new StringBuilder(array.Length+1 * 50);

            sb.Append($"Nom de la variable : {variableName} ; Type : {typeof(T).GetFriendlyName()}[].\n");

            
            if (array.Length == 0)
            {
                sb.Append($"Ce tableau est vide.\n");
            }
            else
            {
                sb.Append("Contenu : \n");
                for (int i = 0; i < array.Length; i++)
                {
                    sb.Append($"    {(array[i] != null ? array[i].ToString() : "Null")}\n");
                }
            }

            return sb.ToString();
        }



        /// <summary>
        /// Renvoie une string affichant le contenu de la liste.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string content<T>(this List<T> list, string variableName)
        {
            if (list == null)
            {
                return $"La liste \"{variableName}\" n'est pas initialisée ; impossible d'obtenir son contenu.\n";
            }


            //+1 pour ajouter le nom de la variable au début
            StringBuilder sb = new StringBuilder((list.Count+1) * 50);
            sb.Append($"Nom de la variable : {variableName} ; Type : List<{typeof(T).GetFriendlyName()}>.\n");


            if (list.Count == 0)
            {
                sb.Append($"Cette liste est vide.\n");
            }
            else
            {
                sb.Append("Contenu : \n");
                for (int i = 0; i < list.Count; i++)
                {
                    sb.Append($"    {(list[i] != null ? list[i].ToString() : "Null")}\n");
                }
            }

            return sb.ToString();
        }

    }
}