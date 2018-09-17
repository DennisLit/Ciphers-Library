using PropertyChanged;

namespace SimpleCiphers
{
    [AddINotifyPropertyChangedInterface]

    public class LoadedFileItem
    {
        public int Id { get; set; }
        public string FileFixedName { get; set; }
        public string FileRealName { get; set; }
        public bool IsLoaded { get; set; }
    }
}
