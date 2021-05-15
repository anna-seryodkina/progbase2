using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using MyLib;
using Terminal.Gui;

class Program
{
    static ActivityRepository activityRepository;
    static ListView allActivitiesListView;

    static void Main(string[] args)
    {
        string databaseFileName = "../db.db";
        SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
        activityRepository = new ActivityRepository(connection);
        //
        Application.Init();

        MenuBar menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem("_New", "", OnNew),
                new MenuItem ("_Quit", "", OnQuit)
            }),
            new MenuBarItem ("_Help", new MenuItem [] {
                new MenuItem("_About", "", OnAbout)
            }),
        });

        Toplevel top = Application.Top;
        Window win = new Window("Bruh")
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 1
        };
        top.Add(menu, win);




        Button nextPageBtn = new Button(1, 3, "<- Prev Page");
        nextPageBtn.Clicked += OnNextPageButton;
        win.Add(nextPageBtn);

        string pagesInfo = $"Total pages: {activityRepository.GetTotalPages()}  Current page: {pageNum}";
        Label pagesInfoLbl = new Label(22, 3, pagesInfo);
        win.Add(pagesInfoLbl);


        Button prevPageBtn = new Button(60, 3, "Next Page ->");
        prevPageBtn.Clicked += OnPrevPageButton;
        win.Add(prevPageBtn);


        Rect frame = new Rect(4, 8, top.Frame.Width, 200);
        allActivitiesListView = new ListView(frame, activityRepository.GetPage(pageNum));
        allActivitiesListView.OpenSelectedItem += OnItemSelected;
        win.Add(allActivitiesListView);


        Button createNewActivityBtn = new Button(2, 20, "create new activity");
        createNewActivityBtn.Clicked += OnCreateButtonClicked;
        win.Add(createNewActivityBtn);


        Application.Run();

    }

    static void OnCreateButtonClicked()
    {
        CreateActivityDialog dialog = new CreateActivityDialog();
        Application.Run(dialog);

        if(!dialog.canceled)
        {
            // switch to [перегляд] window
            Activity activity = dialog.GetActivity();
            long activityId = activityRepository.Insert(activity);
            activity.id = activityId;
        }
    }

    static int pageNum = 1;

    static void OnNextPageButton()
    {
        pageNum++;
        List<Activity> activitiesList = (List<Activity>)allActivitiesListView.Source.ToList();
        activitiesList = activityRepository.GetPage(pageNum);
        allActivitiesListView.SetSource(activitiesList);
    }

    static void OnPrevPageButton()
    {
        pageNum -= 1;
        List<Activity> activitiesList = (List<Activity>)allActivitiesListView.Source.ToList();
        activitiesList = activityRepository.GetPage(pageNum);
        allActivitiesListView.SetSource(activitiesList);
    }


    static void OnItemSelected(ListViewItemEventArgs args)
    {
        // switch to another window
        int itemIndex = args.Item;
        Activity value = (Activity)args.Value;

        throw new NotImplementedException();
    }

    static void OnNew()
    {
        throw new NotImplementedException();
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
