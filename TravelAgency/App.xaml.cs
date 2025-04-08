using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace TravelAgency;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static User currentUser = new User();
    public static readonly string _connectionString = "Host=176.108.249.42;Database=TravelAgency;Username=postgres;Password=0563";
    public static Collection<int> tourInCurrentUser = new Collection<int>();
    public static UserWindows userWindows;
}

public class User
{
    public int? id { get; set; }
    public int? roleId { get; set; }
    public string? login { get; set; }
    public string? password { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public string? surname { get; set; }
    public string? phone { get; set; }
    public string? email { get; set; }
    public bool isBlocked { get; set; }
    
}

