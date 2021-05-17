using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using MyLib;
using Terminal.Gui;

class Program
{
    static ActivityRepository activityRepository;

    public static string[] activities = new string[]
    {
        "walking", "running", "cycling", "swimming", "other"
    };

    static void Main(string[] args)
    {
        string databaseFileName = "../db.db";
        SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
        activityRepository = new ActivityRepository(connection);
        //
        Application.Init();

        Toplevel top = Application.Top;

        MainWindow win = new MainWindow();
        win.SetRepository(activityRepository);

        MenuBar menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem("_New", "", win.OnCreateButtonClicked),
                new MenuItem ("_Quit", "", OnQuit)
            }),
            new MenuBarItem ("_Help", new MenuItem [] {
                new MenuItem("_About", "", OnAbout)
            }),
        });

        top.Add(menu, win);

        Application.Run();
    }

    static void OnAbout() // show dialog with some info
    {
        // zahodit ulitka v bar i govorit < mozshno viski s koloi ? >
        // a barmen ei otvechaet - U nas strogaya politika po otnosheniu k ulitkam. Ulitok ne obsluzshivaem.
        // i vikinul ulitku za dveri.
        // prohodit nedelia. ulitka snova zahodit v bar i govorit <nu i zachem ti eto sdelal ????>
        throw new NotImplementedException();
    }

    static void OnQuit()
    {
        Application.RequestStop();
    }
}
