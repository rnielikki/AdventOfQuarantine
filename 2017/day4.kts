import java.io.File;

File("day4.txt").useLines{
    it.fold (0, { acc, line ->
        acc + line.split(' ').let{
            if(it.size == it.distinct().size){
                1
            }
            else{
                0
            }
        }
    });
}

File("day4.txt").useLines{
    it.fold (0, { acc, line ->
        acc + line.split(' ').let{
            val rearranged = it.map{
                it.toCharArray().sorted().joinToString("")
            }
            if(rearranged.size == rearranged.distinct().size){
                1
            }
            else{
                0
            }
        }
    });
}