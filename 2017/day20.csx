var AllParticles = File.ReadAllLines("day20.txt").Select(line=>Particle.FromString(line)).ToArray();
// part 1 only -------------------------
var min = AllParticles.Min(
    p => p.Acceleration.FromZero()
);
var allMin = AllParticles.Where(p => p.Acceleration.FromZero()==min);

min = allMin.Min(
    p => p.Velocity.FromZero()
);

allMin = AllParticles.Where(p => p.Velocity.FromZero()==min);

min = allMin.Min(
    p => p.Position.FromZero()
);

allMin = AllParticles.Where(p => p.Position.FromZero()==min);

Console.WriteLine(Array.IndexOf(AllParticles, allMin.Single()));
//Part 2 only ---------------------------------------------------------------
int count = 0;

foreach(var particle in AllParticles){
    if(!AllParticles.Any(item=>particle.DoesCollideWith(item))){
        count++;
    }
}
Console.WriteLine(count);
//------------- class and struct -----------------------
class Particle{
    public Vector Position { get; private set; }
    public Vector Velocity { get; private set; }
    public Vector Acceleration { get; }
    public static Particle FromString(string raw){
        var data = raw.Split('>');
        return new Particle(ParseVect(data[0]), ParseVect(data[1]), ParseVect(data[2]));

        Vector ParseVect(string s){
            string[] values = s.Substring(s.IndexOf('<')+1)
            .Split(',');
            return new Vector(int.Parse(values[0]),int.Parse(values[1]),int.Parse(values[2]));
        }
    }
    public Particle(Vector position, Vector velocity, Vector acceleration){
        Position = position;
        Velocity = velocity;
        Acceleration = acceleration;
    }
    public void MoveStep()
    {
        Velocity+=Acceleration;
        Position+=Velocity;
    }
    public override string ToString()
    {
        return $"P : {Position.ToString()} / V: {Velocity.ToString()} / A: {Acceleration.ToString()}";
    }
    //GO, QUAD
    public bool DoesCollideWith(Particle other){
        if(other == this) return false;
        Vector AccDiff = Acceleration - other.Acceleration;
        Vector VeloDiff = Velocity - other.Velocity;
        Vector PosDiff = Position -other.Position;
        return
        GetSolution(
            AccDiff.X,
            VeloDiff.X,
            PosDiff.X
        ).
        Where(res=>
            GetApply(
                AccDiff.Y,
                VeloDiff.Y,
                PosDiff.Y,
                res
            ) &&
            GetApply(
                AccDiff.Z,
                VeloDiff.Z,
                PosDiff.Z,
                res
            )
        ).Any();
    }
    private IEnumerable<int> GetSolution(int a, int v, int s) => Solution(a*0.5, v+a*0.5, s);
    private IEnumerable<int> Solution(double a, double b, int c){
        List<int> solutions = new();
        double checkRoot = b*b-4*a*c;
        if(checkRoot<0) return solutions;
        double solution1 = (Math.Sqrt(checkRoot)-b)/(2*a);
        double solution2 = (-Math.Sqrt(checkRoot)-b)/(2*a);
        if(checkRoot==0){
            if(solution1 >= 0 && solution1%1==0){
                solutions.Add((int)solution1);
            }
            return solutions;
        }
        if(solution1 >= 0 && solution1%1==0){
            solutions.Add((int)solution1);
        }
        if(solution2 >= 0 && solution2%1==0){
            solutions.Add((int)solution2);
        }
        return solutions;
    }
    private bool GetApply(int a, int v, int s, int t) => Apply(a*0.5, v+a*0.5, s, t);
    private bool Apply(double a, double b, int c, int t) =>  a*t*t+b*t+c == 0;
}
struct Vector{
    public int X {get;}
    public int Y {get;}
    public int Z {get;}
    public static Vector Zero = new Vector(0,0,0);
    public Vector(int x, int y, int z){
        X = x;
        Y = y;
        Z = z;
    }
    public static Vector operator +(Vector a, Vector b) => new Vector(a.X+b.X, a.Y+b.Y, a.Z+b.Z);
    public static Vector operator -(Vector a, Vector b) => new Vector(a.X-b.X, a.Y-b.Y, a.Z-b.Z);
    public override string ToString()
    {
        return $"[{X}, {Y}, {Z}]";
    }
    public int FromZero() => GetDistance(Vector.Zero);
    public int GetDistance(Vector Another){
        var subst = this-Another;
        return Math.Abs(subst.X)+Math.Abs(subst.Y)+Math.Abs(subst.Z);
    }
}