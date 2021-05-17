using Terminal.Gui;
using MyLib;
using System.Collections;
using System;
using System.Collections.Generic;

public class MainWindow : Window
{
    private ListView allActivitiesListView;

    private ActivityRepository activityRepository;
    private Label totalPagesLbl;
    private Label pageLbl;
    private Button prevPageBtn;
    private Button nextPageBtn;
    private Label emptyLabel;

    private int pageSize = 5;
    private int pageNum = 1;
    public MainWindow()
    {
        this.Title = "My App";

        prevPageBtn = new Button(2, 4, "Prev");
        prevPageBtn.Clicked += OnPrevPageButton;

        pageLbl = new Label("?")
        {
            X = Pos.Right(prevPageBtn) + 2,
            Y = Pos.Top(prevPageBtn),
            Width = 5,
        };

        totalPagesLbl = new Label("?")
        {
            X = Pos.Right(pageLbl) + 2,
            Y = Pos.Top(prevPageBtn),
            Width = 5,
        };

        nextPageBtn = new Button("Next")
        {
            X = Pos.Right(totalPagesLbl) + 2,
            Y = Pos.Top(prevPageBtn),
        };
        nextPageBtn.Clicked += OnNextPageButton;
        this.Add(prevPageBtn, pageLbl, totalPagesLbl, nextPageBtn);


        Rect frame = new Rect(4, 8, 40, 200);
        allActivitiesListView = new ListView(new List<Activity>())
        {
            Width = Dim.Fill(),
            Height = Dim.Fill(),
        };
        allActivitiesListView.OpenSelectedItem += OnOpenActivity;
        FrameView frameView = new FrameView("Activities")
        {
            X = 2,
            Y = 6,
            Width = Dim.Fill() - 4,
            Height = pageSize + 2,
        };
        frameView.Add(allActivitiesListView);
        this.Add(frameView);


        Button createNewActivityBtn = new Button(2, 16, "create new activity");
        createNewActivityBtn.Clicked += OnCreateButtonClicked;
        this.Add(createNewActivityBtn);

        emptyLabel = new Label("Database is empty")
        {
            X = 4, Y = 14, Visible = false,
        };
        this.Add(emptyLabel);
    }
    public void SetRepository(ActivityRepository repo)
    {
        this.activityRepository = repo;
        this.UpdateCurrentPage();
    }

    private void UpdateCurrentPage()
    {
        int totalPages = (int)activityRepository.GetTotalPages(pageSize);
        if(pageNum > totalPages && pageNum > 1)
        {
            pageNum = totalPages;
        }
        this.pageLbl.Text = pageNum.ToString();

        if (totalPages == 0)
        {
            emptyLabel.Visible = true;
        }
        else
        {
            emptyLabel.Visible = false;
        }
        
        if (totalPages == 0)
        {
            totalPages = 1;
        }
        this.totalPagesLbl.Text = totalPages.ToString();

        this.allActivitiesListView.SetSource(activityRepository.GetPage(this.pageNum, this.pageSize));

        prevPageBtn.Visible = (pageNum != 1);
        nextPageBtn.Visible = (pageNum != totalPages );
    }

    private void OnOpenActivity(ListViewItemEventArgs args)
    {
        Activity value = (Activity)args.Value;
        OpenActivityDialog dialog = new OpenActivityDialog();
        dialog.SetActivity(value);

        Application.Run(dialog);

        if(dialog.deleted)
        {
            bool result = activityRepository.Delete(value.id);
            if(result)
            {
                int pages = (int)activityRepository.GetTotalPages(pageSize);
                if(pageNum > pages && pageNum > 1)
                {
                    pageNum -= 1;
                }
                this.UpdateCurrentPage();
            }
            else
            {
                MessageBox.ErrorQuery("Delete activity", "Can not delete activity", "Ok");
            }
        }

        if(dialog.updated)
        {
            bool result = activityRepository.Update(value.id, dialog.GetActivity());
            if(result)
            {
                allActivitiesListView.SetSource(activityRepository.GetPage(pageNum, pageSize));
            }
            else
            {
                MessageBox.ErrorQuery("Update activity", "Can not update activity", "Ok");
            }
        }
    }

    public void OnCreateButtonClicked()
    {
        CreateActivityDialog dialog = new CreateActivityDialog();
        Application.Run(dialog);

        if(!dialog.canceled)
        {
            Activity activity = dialog.GetActivity();
            long activityId = activityRepository.Insert(activity);
            activity.id = activityId;

            UpdateCurrentPage();
        }
    }

    private void OnNextPageButton()
    {
        int pages = (int)activityRepository.GetTotalPages(pageSize);
        if(pageNum >= pages)
        {
            return;
        }
        this.pageNum += 1;
        this.UpdateCurrentPage();
    }

    private void OnPrevPageButton()
    {
        if(pageNum <= 1)
        {
            return;
        }
        this.pageNum -= 1;
        this.UpdateCurrentPage();
    }
}