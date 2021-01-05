import java.io.File;

//val separator = ' ';
val separator = '\t';

File("day2.txt").useLines{
    it.fold (0, { acc, line ->
        acc + line.split(separator).map{ it.toInt() }.let{
            it.maxOrNull()!! - it.minOrNull()!!
        }
    });
}

File("day2.txt").useLines{
    it.fold (0, { acc, line ->
        acc+line.split(separator).map{ it.toInt() }.let{
            for(current in it){
                val filtered = it.filter{ it!=current && it%current == 0 };
                if(filtered.any()){
                    return@let filtered.first()/current;
                }
            }
            0
        }
    });
}