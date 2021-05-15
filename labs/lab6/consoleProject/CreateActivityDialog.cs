using MyLib;
using Terminal.Gui;
using System;

public class CreateActivityDialog : Dialog
{
    public bool canceled;

    private RadioGroup activityTypeRadioGr;
    private TextField activityNameInput;
    private TextField commentInput;
    private TextField distanceInput;


    public CreateActivityDialog()
    {
        this.Title = "create new activity";
        Button okBtn = new Button("Ok");
        okBtn.Clicked += OnCreateDialogSubmit;

        Button cancelBtn = new Button("Cancel");
        cancelBtn.Clicked += OnCreateDialogCancelled;

        this.AddButton(cancelBtn);
        this.AddButton(okBtn);



        int rightColumnX = 20;



        Label activityTypeLbl = new Label(2, 2, "Type:");
        activityTypeRadioGr = new RadioGroup()
        {
            X = rightColumnX, Y = Pos.Top(activityTypeLbl),
            RadioLabels = new NStack.ustring[]{"walking", "running", "cycling", "swimming", "other"},
        };
        this.Add(activityTypeLbl, activityTypeRadioGr);



        Label activityNameLbl = new Label(2, 8, "Name:");
        activityNameInput = new TextField("")
        {
            X = rightColumnX, Y = Pos.Top(activityNameLbl), Width = 40,
        };
        this.Add(activityNameLbl, activityNameInput);



        Label commentLbl = new Label(2, 10, "Comment:");
        commentInput = new TextField("")
        {
            X = rightColumnX, Y = Pos.Top(commentLbl), Width = 40,
        };
        this.Add(commentLbl, commentInput);



        Label distanceLbl = new Label(2, 12, "Distance:");
        distanceInput = new TextField("")
        {
            X = rightColumnX, Y = Pos.Top(distanceLbl), Width = 40,
        };
        this.Add(distanceLbl, distanceInput);
    }

    public Activity GetActivity()
    {
        return new Activity()
        {
            type = activityTypeRadioGr.SelectedItem.ToString(), // returns index
            name = activityNameInput.Text.ToString(),
            comment = commentInput.Text.ToString(),
            distance = Convert.ToInt32(distanceInput.Text.ToString()),
        };
    }

    private void OnCreateDialogCancelled()
    {
        this.canceled = true;
        Application.RequestStop();
    }

    private void OnCreateDialogSubmit()
    {
        this.canceled = false;
        Application.RequestStop();
    }
}