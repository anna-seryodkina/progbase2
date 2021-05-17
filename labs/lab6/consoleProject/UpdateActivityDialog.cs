using MyLib;

public class UpdateActivityDialog : CreateActivityDialog
{
    public UpdateActivityDialog()
    {
        this.Title = "Edit activity";
    }

    public void SetActivity (Activity a)
    {
        this.activityTypeRadioGr.Text = a.type;
        this.activityNameInput.Text = a.name;
        this.commentTextView.Text = a.comment;
        this.distanceInput.Text = a.distance.ToString();
    }

}