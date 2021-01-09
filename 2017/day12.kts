import java.io.File;

File("day12.txt").readLines().let{
    
    
    val input = it.map{ it.split('>')[1].split(',').map{ it.trim().toIntOrNull()!! } }
    val grouped = HashSet<HashSet<Int>>();
    val unused = (0..(input.size-1)).toMutableList()

    fun search(index:Int, connected:HashSet<Int>):HashSet<Int>{
        if(!connected.contains(index)){
            connected.add(index);
            unused.removeAt(unused.indexOf(index));
            for(item in input[index]){
                search(item, connected);
            }
        }
        return connected;
    }
    while(unused.size > 0){
        grouped.add(search(unused.first(), HashSet<Int>()))
    }
    val first = (grouped.find { it.contains(0) })!!.size
    println(first)
    //second
    grouped.size
}