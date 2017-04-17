using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextureAtlas {

    public static readonly TextureAtlas _Instance = new TextureAtlas();
    public static Texture2D _ATLAS { get; private set; }
    public void CreateAtlas()
    {
        string[] _Images = Directory.GetFiles("textures/blocks/");
        int PixelWidth = 16;
        int PixelHeight = 16; 
        int atlaswidth = Mathf.CeilToInt((Mathf.Sqrt(_Images.Length) + 1)) * PixelWidth;
        int atlasheight = Mathf.CeilToInt((Mathf.Sqrt(_Images.Length)  + 1)) * PixelHeight;
        Texture2D Atlas = new Texture2D(atlaswidth, atlasheight);
        int count = 0;
        for (int x = 0; x < atlaswidth / PixelWidth; x++)
        {
            for (int y = 0; y < atlaswidth / PixelWidth; y++)
            {

                if (count > _Images.Length - 1)
                    goto end;
                Texture2D temp = new Texture2D(0, 0);



                temp.LoadImage(File.ReadAllBytes(_Images[count]));
                Atlas.SetPixels(x * PixelWidth, y * PixelHeight, PixelWidth, PixelHeight, temp.GetPixels());

                float startx = x * PixelWidth;
                float starty = y * PixelHeight;
                float perpixelratiox = 1.0f / Atlas.width;
                float perpixelratioy = 1.0f / Atlas.height;
                startx *= perpixelratiox;
                starty *= perpixelratioy;
                float endx = startx + (perpixelratiox * PixelWidth);
                float endy = starty + (perpixelratioy * PixelHeight);

                UvMap m = new UvMap(_Images[count], new Vector2[] {
                    new Vector2(startx,starty),
                    new Vector2(startx,endy),
                    new Vector2(endx,starty),
                    new Vector2(endx,endy)


                });

                m.Register();

                count++;


            }
        }

    end:;
        _ATLAS = Atlas;
        File.WriteAllBytes("textures/atlas.png", Atlas.EncodeToPNG());

    }
}
