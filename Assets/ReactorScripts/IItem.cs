namespace ReactorScripts
{
    public interface IItem
    {
        string Name { get; set; }
        TypeItem Type { get; set; }
        int Repair { get; set; }
    }
}
