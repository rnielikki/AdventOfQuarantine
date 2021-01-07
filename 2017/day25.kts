import java.io.File;

File("day25.txt").readLines().let{
    val lines = it.toTypedArray<String>();
    var start = lines[0].takeLast(2)[0];
    val max = lines[1].filter{ it.isDigit() }.toInt();
    val match = lines.withIndex().filter{
        val (_, value) = it;
        value.length>0 && (value[0] == 'I');
    }.associate{
        val (index, value) = it;
        value.takeLast(2)[0] to index;
    }
    var step = 0;
    val tape = HashSet<Int>();
    var cursor = 0;
    var currentLine = match.get(start)!!;
    while(step < max){ 
        if(lines[currentLine].length==0){
            currentLine++;
            continue;
        }
        if((lines[++currentLine].takeLast(2)[0]=='1') != tape.contains(cursor)){
            currentLine+=4;
        }
        //line one
        if(lines[++currentLine].takeLast(2)[0]=='1'){
            tape.add(cursor);
        }
        else{
            tape.remove(cursor);
        }
        //line two
        if(lines[++currentLine].takeLast(3)[0]=='f'){
            cursor--;
        }
        else{
            cursor++;
        }
        //line three
        val next = lines[++currentLine].takeLast(2)[0];
        currentLine = match.get(next)!!;
        step++;
    }
    tape.size;
}