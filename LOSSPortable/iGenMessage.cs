using System;

public interface GenMessage
{
    public string Subject { set; get; }

    public string Title { set; get; }

    public string Text { set; get; }

    public string Time { set; get; }

    public string ToFrom { set; get; }

    public string Sender { set; get; }
}

