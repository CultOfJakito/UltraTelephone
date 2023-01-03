using HarmonyLib;
using UnityEngine;

namespace UltraTelephone.Patches
{
    [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.GetHurt))]
    public class Temperz_Inject_HurtSounds_And_Other_Stuff_Like_Funnys
    {
        [HarmonyPrefix]
        public static bool Prefix(NewMovement __instance)
        {
            AudioSwapper.SwapHurtSource(Traverse.Create(__instance).Field("hurtAud").GetValue<AudioSource>());
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix(NewMovement __instance)
        {
            if (!__instance.dead)
                return;
            string[] funnys = new string[]
            {
                "https://knowyourmeme.com/memes/trollface",
                "https://i.kym-cdn.com/entries/icons/original/000/033/228/cover3.jpg",
                "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                "https://www.youtube.com/watch?v=5Q_2nrp9OTg",
                "https://www.youtube.com/watch?v=3HXmFDMgVqI",
                "https://en.wiktionary.org/wiki/epic_fail",
                "https://www.google.com/url?sa=i&url=https%3A%2F%2Ftenor.com%2Fview%2Famong-us-twerk-yellow-ass-thang-gif-18983570&psig=AOvVaw180CaYxHczgmhx1l4QUOsv&ust=1672849130687000&source=images&cd=vfe&ved=0CA4QjRxqFwoTCJCoxOLmq_wCFQAAAAAdAAAAABAE", // If someone deems this as too distateful feel free to remove it
                "https://preview.redd.it/8c6qq2x55gk91.jpg?width=640&crop=smart&auto=webp&s=bfed4215cbd4b38e9ddd095e567496590b3461ea",
                "https://youtu.be/kESW8-26PXk",
                "https://www.youtube.com/playlist?list=PLtr1CuIZfdMAwqqRa29SrZhuwzPyKOGqw",
                "https://www.youtube.com/watch?v=JFNOmoVCIiU"
            }; 
            Application.OpenURL(funnys[new System.Random().Next(0, funnys.Length - 1)]);
        }
    }
}
