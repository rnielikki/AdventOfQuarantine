class Item{
    private static Dictionary<string, Item> _allItems;
    public string Name { get; }
    public int Weight { get; }
    public int TotalWeight { get; private set; }
    HashSet<Item> Children { get; }
    HashSet<string> _childrenNames { get; }
    static Item _root;
    Item Parent;
    public Item(string name, int weight, IEnumerable<string> children){
        Name = name;
        Weight = weight;
        _childrenNames = children.ToHashSet();
        Children = new();
    }
    private static Dictionary<string, Item> InitAllItems() =>
        File.ReadAllLines("day7.txt").Select(item=>{
            var items = item.Split(' ', 4);
            IEnumerable<string> children;
            if(items.Length>2){
                children = items[3].Split(',').Select(s=>s.Trim());
            }
            else{
                children = Enumerable.Empty<string>();
            }
            return new Item(items[0], int.Parse(items[1][1..^1]), children);
        }).ToDictionary(kv=>kv.Name, kv=>kv);
    public static void Init(){
        _allItems = InitAllItems();
        ConnectAllParents();
    }
    private static void ConnectAllParents(){
        foreach(var item in _allItems.Values){
            item.AddChildren();
            item.ConnectParent();
        }
        _root.CountWeight();
    }
    public static void GetBalance() => _root.CheckBalance();
    private void AddChildren(){
        foreach(var child in _childrenNames){
            Children.Add(_allItems[child]);
        }
    }
    private void ConnectParent(){
        var parent = _allItems.SingleOrDefault(val=>val.Value._childrenNames.Contains(Name));
        if(parent.Equals(default(KeyValuePair<string, Item>))){
            //Part 1
            Console.WriteLine(Name);
            _root = _allItems[Name];
        }
        else{
            Parent = parent.Value;
        }
    }
    private int CountWeight(){
        TotalWeight = Weight + Children.Sum(item=>item.CountWeight());
        return TotalWeight;
    }
    private bool CheckBalance(){
        if(!Children.Any()) return true;
        var weightGroup = Children.GroupBy(child=>child.TotalWeight).ToDictionary(kv=>kv.Key, kv=>kv.ToList());
        if(weightGroup.Count > 1){
            bool childrenBalanced = true;
            foreach(var child in Children){
                if(!child.CheckBalance()){
                    childrenBalanced = false;
                }
            }
            if(!childrenBalanced) return false;
            var unbalancedKvPair = weightGroup.Where(kv=>kv.Value.Count==1).Single();
            var wrongWeight = unbalancedKvPair.Key;
            var rightWeight = weightGroup.Where(kv=>kv.Value.Count>1).Single().Key;
            var targetWeight = unbalancedKvPair.Value.Single().Weight;
            //var rightWeight = weights.Where(num=>num!=wrongWeight).Distinct();
            
            //Two
            Console.WriteLine(wrongWeight + " need to be "+rightWeight);
            Console.WriteLine("and it results : "+(targetWeight+rightWeight-wrongWeight));
            return false;
        }
        return true;
    }
}

Item.Init();
Item.GetBalance();