public class SceneData
{
    public string background;
    public string backgroundMusic;
    public string[] characters;

    public override string ToString()
    {
        return "{ bg: " + background + " bgmusic: " + backgroundMusic + " chara: " + string.Join(',', characters) + " }";
    }
}
