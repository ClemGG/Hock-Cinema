using UnityEngine;

namespace Clement.Utilities
{

    public static class Maths
    {

        /// <summary>
        /// Pour changer une valeur d'un intervalle à un autre
        /// </summary>
        /// <param name="f"></param>
        /// <param name="ancienX"></param>
        /// <param name="ancienY"></param>
        /// <param name="nouveauX"></param>
        /// <param name="nouveauY"></param>
        /// <returns></returns>
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }


        public static float RandomV2(this float f, Vector2 interval)
        {
            return Random.Range(interval.x, interval.y);
        }


        /// <summary>
        /// Pareil que le Mathf.Approximately, mais celui-là permet d'utiliser un intervalle de sensibilité. Utile quand on ne sait pas si a sera plus grand ou plus petit que b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static bool FastApproximate(float a, float b, float threshold)
        {
            return ((a < b) ? (b - a) : (a - b)) <= threshold;
        }


        /// <summary>
        /// Similaire à FastApproximately, mais ne s'utilise que si a est strictement plus grand que b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static bool FastApproximatelyWithFirstArgumentAsSuperiorStrict(float a, float b, float threshold)
        {
            return (a - b) <= threshold;
        }


        /// <summary>
        /// Similaire à FastApproximately, mais ne s'utilise que si a est strictement plus petit que b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static bool FastApproximatelyWithFirstArgumentAsInferiorStrict(float a, float b, float threshold)
        {
            return (b - a) <= threshold;
        }
    }
}