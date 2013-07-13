using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace BarbieBear
{
    public static class TextureBin
    {
        private static ContentManager cm;

        private static List<string> names = new List<string>
            {
                "Pixel", 
                "bear",
                "background",
                "barbie",
                "barbie_leftpunch", 
                "barbie_upleftpunch", 
                "barbie_uppunch", 
                "barbie_uprightpunch", 
                "barbie_rightpunch",
                "sparkle",
            };

        private static List<string> soundNames = new List<string>
            {
                "barbie1",
                "barbie2",
                "barbie3",
                "barbie4",
                "barbie5",
                "bear",
                "punch1",
                "punch2",
                "punch3",
                "punch4",
            };

        private static Dictionary<string, Texture2D> texDic = new Dictionary<string, Texture2D>();
        private static Dictionary<string, SoundEffect> soundDic = new Dictionary<string, SoundEffect>();
 
        public static Texture2D Pixel { get { return TextureBin.Get("Pixel"); } }

        public static void LoadContent(ContentManager cm)
        {
            TextureBin.cm = cm;

            foreach (var name in names)
            {
                texDic[name] = cm.Load<Texture2D>("Textures/" + name);
            }

            foreach (var soundName in soundNames)
            {
                soundDic[soundName] = cm.Load<SoundEffect>("Sounds/" + soundName);
            }

            //foreach (string name in Directory.GetFiles("Content/Textures/", "*.xnb"))
            //{
            //    string filename = name.Substring(name.LastIndexOf("/")+1, name.LastIndexOf(".") - (name.LastIndexOf("/")+1));
            //    texDic[filename] = cm.Load<Texture2D>("Textures/" + filename);
            //}

            //foreach (string name in Directory.GetFiles("Content/Sounds/", "*.xnb"))
            //{
            //    string filename = name.Substring(name.LastIndexOf("/")+1, name.LastIndexOf(".") - (name.LastIndexOf("/")+1));
            //    soundDic[filename] = cm.Load<SoundEffect>("Sounds/" + filename);
            //}
        }

        public static Texture2D Get(string name)
        {
            return texDic[name];
        }

        public static SpriteFont GetFont(string name)
        {
            return cm.Load<SpriteFont>("Fonts/" + name);
        }

        public static void PlaySound(string name)
        {
            soundDic[name].Play();
        }
    }
}