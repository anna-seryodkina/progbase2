using MyLib;
using Terminal.Gui;
using System;

public class OpenActivityDialog : Dialog
{
    public bool deleted;
    public bool updated;
    private Label activityTypeLabel;
    private TextField activityNameInput;
    private TextView commentTextView;
    private TextField distanceInput;

    protected Activity activity;


    public OpenActivityDialog()
    {
        this.Title = "open activity";
        this.Width = Dim.Percent(80);
        this.Height = Dim.Percent(80);

        Button updateBtn = new Button(2, 15, "Update");
        updateBtn.Clicked += OnUpdate;

        Button deleteBtn = new Button("Delete")
        {
            X = Pos.Right(updateBtn) + 2,
            Y = Pos.Top(updateBtn),
        };
        deleteBtn.Clicked += OnDelete;
        this.Add(updateBtn, deleteBtn);

        Button backBtn = new Button("Back");
        backBtn.Clicked += OnBack;
        this.AddButton(backBtn);


        int rightColumnX = 20;

        Label activityTypeLbl = new Label(2, 2, "Type:");
        activityTypeLabel = new Label()
        {
            X = rightColumnX, Y = Pos.Top(activityTypeLbl),
        };
        this.Add(activityTypeLbl, activityTypeLabel);



        Label activityNameLbl = new Label(2, 4, "Name:");
        activityNameInput = new TextField("")
        {
            X = rightColumnX, Y = Pos.Top(activityNameLbl), Width = 40,
            ReadOnly = true,
        };
        this.Add(activityNameLbl, activityNameInput);


        Label distanceLbl = new Label(2, 6, "Distance:");
        distanceInput = new TextField("")
        {
            X = rightColumnX, Y = Pos.Top(distanceLbl), Width = 40,
            ReadOnly = true,
        };
        this.Add(distanceLbl, distanceInput);


        Label commentLbl = new Label(2, 8, "Comment:");
        commentTextView = new TextView()
        {
            X = rightColumnX, Y = Y = Pos.Top(commentLbl),
            Width = 40, Height = 5,
            ReadOnly = true,
        };
        this.Add(commentLbl, commentTextView);

    }

    public void SetActivity(Activity a)
    {
        this.activity = a;
        this.activityTypeLabel.Text = a.type;
        this.activityNameInput.Text = a.name;
        this.commentTextView.Text = a.comment;
        this.distanceInput.Text = a.distance.ToString();
    }

    public Activity GetActivity()
    {
        return this.activity;
    }

    private void OnBack() // should return to main window !
    {
        Application.RequestStop();
    }

    private void OnUpdate()
    {
        UpdateActivityDialog dialog = new UpdateActivityDialog();
        dialog.SetActivity(this.activity);

        Application.Run(dialog);
        if(!dialog.canceled)
        {
            Activity updatedActivity = dialog.GetActivity();
            this.updated = true;
            this.SetActivity(updatedActivity);
            Application.RequestStop();
        }
    }

    private void OnDelete()
    {
        int index = MessageBox.Query("Delete book", "Are you sure?", "No", "Yes");
        if(index == 1)
        {
            this.deleted = true;
            Application.RequestStop();
        }
    }

}