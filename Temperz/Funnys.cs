using UnityEngine;

namespace UltraTelephone.Temperz
{
    public static class Funnys
    {
        private static System.Random rnd = new System.Random();
        static string[] funnys = new string[]
            {
                "https://knowyourmeme.com/memes/trollface",
                "https://i.kym-cdn.com/entries/icons/original/000/033/228/cover3.jpg",
                "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
                "https://www.youtube.com/watch?v=5Q_2nrp9OTg",
                "https://www.youtube.com/watch?v=3HXmFDMgVqI",
                "https://en.wiktionary.org/wiki/epic_fail",
                "https://tenor.com/en-GB/view/among-us-twerk-yellow-ass-thang-gif-18983570", // I did find your comment distasteful so I removed it :)
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
                "https://pbs.twimg.com/media/FJb8kqQUYAEUy2a?format=jpg&name=small",
                "https://youtu.be/Ds14zhfHEvE",
                "https://www.youtube.com/watch?v=_LjN3UclYzU",
                "https://www.youtube.com/shorts/cPWH4CzxaFk",
                "https://www.youtube.com/watch?v=wcpQ3aarHRU",
                "https://www.youtube.com/watch?v=n-aL5da34SI",
                "https://www.youtube.com/watch?v=TrE0LRjIYQY",
                "https://www.youtube.com/watch?v=ESUaRUNYQ74",
                "https://www.youtube.com/watch?v=x3fmTe_9cNA",
                "https://www.youtube.com/watch?v=AEtbFm_CjE0"
            };

        public static void OpenFunny()
        {
            Application.OpenURL(funnys[rnd.Next(0, funnys.Length)]);
        }
    }
}
