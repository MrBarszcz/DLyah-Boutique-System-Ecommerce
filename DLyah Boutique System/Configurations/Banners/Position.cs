namespace DLyah_Boutique_System.Configurations.Banners;

public class Position {
    public string Name { get; set; }
    public string Description { get; set; }
    public string DisplayType { get; set; }
    public int ContainerMaxWidth { get; set; }
    public int ContainerMaxHeight { get; set; }
    public List<LayoutRule> LayoutRules { get; set; }
}