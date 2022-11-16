using System;

[Serializable]
public class Human
{
    public string id;
    public string first_name;
    public string last_name;
    public string email;
    public string gender;
    public string ip_address;
    public string address;

    public string ID
    {
        get { return id; }
        set { id = value; }
    }

    public string First_name
    {
        get { return first_name; }
        set { first_name = value; }
    }

    public string Last_name
    {
        get { return last_name; }
        set { last_name = value; }
    }

    public string Email
    {
        get { return email; }
        set { email = value; }
    }

    public string Gender
    {
        get { return gender; }
        set { gender = value; }
    }

    public string Ip_address
    {
        get { return ip_address; }
        set { ip_address = value; }
    }

    public string Address
    {
        get { return address; }
        set { address = value; }
    }

}
