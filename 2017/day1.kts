import java.io.File;

//1
File("day1.txt").readText().toCharArray().let{
    var before = it.last();
    var count = 0;
    for(item in it){
        if(item==before){
            count+=item.toString().toInt();
        }
        before = item;
    }
    println(count)
};

//2
File("day1.txt").readText().toCharArray().let{
    var part1 = it.slice(IntRange(0,it.size/2-1));
    var part2 = it.slice(IntRange(it.size/2,it.size-1));
    var count = 0;
    for(i in 0..(it.size/2-1)){
        if(part1[i]==part2[i]){
            count+=part1[i].toString().toInt()*2;
        }
    }
    println(count)
};