namespace ChocolateEditor.Models;

public sealed class Bank
{
    public int Index { get; set; }
    public string Name { get; set; } = "Banco";
    public FootswitchConfig A { get; set; } = new() { Id = FootswitchId.A };
    public FootswitchConfig B { get; set; } = new() { Id = FootswitchId.B };
    public FootswitchConfig C { get; set; } = new() { Id = FootswitchId.C };
    public FootswitchConfig D { get; set; } = new() { Id = FootswitchId.D };

    public FootswitchConfig this[FootswitchId id] => id switch
    {
        FootswitchId.A => A,
        FootswitchId.B => B,
        FootswitchId.C => C,
        FootswitchId.D => D,
        _ => A
    };

    public IEnumerable<FootswitchConfig> Switches
    {
        get
        {
            yield return A;
            yield return B;
            yield return C;
            yield return D;
        }
    }

    public Bank Clone()
    {
        return new Bank
        {
            Index = Index,
            Name = Name,
            A = A.Clone(),
            B = B.Clone(),
            C = C.Clone(),
            D = D.Clone()
        };
    }

    public void CopyFrom(Bank source, bool copyA, bool copyB, bool copyC, bool copyD, bool copyShort, bool copyLong)
    {
        if (copyA) A.CopyFrom(source.A, copyShort, copyLong);
        if (copyB) B.CopyFrom(source.B, copyShort, copyLong);
        if (copyC) C.CopyFrom(source.C, copyShort, copyLong);
        if (copyD) D.CopyFrom(source.D, copyShort, copyLong);
    }

    public string ListLabel => $"{Index + 1:00}  {Name}";
}
