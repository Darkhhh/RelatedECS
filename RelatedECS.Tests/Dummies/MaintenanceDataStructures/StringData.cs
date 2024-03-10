using System.Text;

namespace RelatedECS.Tests.Dummies.MaintenanceDataStructures;

internal class StringData
{
    private readonly StringBuilder _stringBuilder = new();

    public StringData Append(string str)
    {
        _stringBuilder.Append(str);
        return this;
    }

    public override string ToString() => _stringBuilder.ToString();
}
