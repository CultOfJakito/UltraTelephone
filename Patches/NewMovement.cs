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
                "https://tenor.com/en-GB/view/among-us-twerk-yellow-ass-thang-gif-18983570", // If someone deems this as too distateful feel free to remove it
                "https://preview.redd.it/8c6qq2x55gk91.jpg?width=640&crop=smart&auto=webp&s=bfed4215cbd4b38e9ddd095e567496590b3461ea",
                "https://youtu.be/kESW8-26PXk",
                "https://www.youtube.com/playlist?list=PLtr1CuIZfdMAwqqRa29SrZhuwzPyKOGqw",
                "https://www.youtube.com/watch?v=JFNOmoVCIiU",
                "https://www.youtube.com/watch?v=kMFYSUFlryo",
                "https://youtu.be/vVT2MUHRe_k?t=13",
                "https://www.youtube.com/watch?v=J83lw0eFIJA",
                "https://www.youtube.com/watch?v=Ta2CK4ByGsw",
                "https://www.youtube.com/watch?v=gDjMZvYWUdo",
                "https://www.youtube.com/watch?v=rkOib-MmTds",
                "https://youtu.be/SBMXj0tw0WU",
                "https://en.wikipedia.org/wiki/List_of_individual_trees",
                "https://store.steampowered.com/app/714010/Aim_Lab/",
                "https://cdn.discordapp.com/attachments/432329547023908884/1061639113310216314/9k.png",
                "https://www.youtube.com/watch?v=THWoFDrXsTU",
                "https://www.youtube.com/watch?v=QTldkmca6bE",
                "https://www.youtube.com/watch?v=Ml7iVgcRZzo",
                "https://youtu.be/Ak0EGTB4APM",
                "https://pbs.twimg.com/media/FJb8kqQUYAEUy2a?format=jpg&name=small"

            }; 
            Application.OpenURL(funnys[new System.Random().Next(0, funnys.Length - 1)]);
        }
    }
}
