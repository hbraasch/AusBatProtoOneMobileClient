using System;

public class DbaseVersion
{
    public int major { get; set; }
    public int minor { get; set; }
    public int patch { get; set; }

    public DbaseVersion(int major, int minor, int patch)
    {
        this.major = major;
        this.minor = minor;
        this.patch = patch;
    }

    /// <summary>
    /// Expects string of format "<major>.<minor>.<patch>"
    /// </summary>
    /// <param name="version"></param>
    public DbaseVersion(string version)
    {
        var values = version.Split('.');
        if (values.Length != 3)
        {
            throw new ApplicationException("Version  string is incorrectly formatted");
        }
        try
        {
            this.major = int.Parse(values[0]);
            this.minor = int.Parse(values[1]);
            this.patch = int.Parse(values[2]); ;
        }
        catch (Exception)
        {
            throw new ApplicationException("Version  string is incorrectly formatted");
        }
    }

    public override bool Equals(object obj)
    {
        if (major == ((DbaseVersion)obj).major && minor == ((DbaseVersion)obj).minor && patch == ((DbaseVersion)obj).patch) return true;
        return false;
    }

    public static bool operator ==(DbaseVersion a, DbaseVersion b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(DbaseVersion a, DbaseVersion b)
    {
        return !(a.Equals(b));
    }

    public static bool operator >(DbaseVersion a, DbaseVersion b)
    {
        if (a.major > b.major) return true;
        if (a.minor > b.minor) return true;
        if (a.patch > b.patch) return true;
        return false;
    }

    public static bool operator <(DbaseVersion a, DbaseVersion b)
    {
        if (b.major > a.major) return true;
        if (b.minor > a.minor) return true;
        if (b.patch > a.patch) return true;
        return false;
    }

    public static bool operator <=(DbaseVersion a, DbaseVersion b)
    {
        if (a == b) return true;
        return a < b;
    }

    public static bool operator >=(DbaseVersion a, DbaseVersion b)
    {
        if (a == b) return true;
        return a > b;
    }

    public override string ToString()
    {
        return $"{major}.{minor}.{patch}";
    }

    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }

    public static DbaseVersion MinValue => new DbaseVersion(0, 0, 0);
    public static DbaseVersion StartValue => new DbaseVersion(1, 0, 0);
}