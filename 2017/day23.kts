import java.io.File;

File("day23.txt").readLines().let{
    val variables = HashMap<Char, Int>();
    var multiple = 0;
    for(i in 0..7){
        variables.put((97+i).toChar(),0);
    }
    fun getVar(value:String):Int{
        return (value.toIntOrNull() ?: variables[value[0]])!!;
    }
    var currentLine = 0;
    while(currentLine < it.size){
        val cut = it[currentLine].split(' ');
        val target = cut[1];
        var value = getVar(cut[2]);
        when(cut[0]){
            "set"->
                variables.set(target[0], value);
            "sub"->
                variables.set(target[0], variables.get(target[0])!!-value);
            "mul"->
                { multiple++; variables.set(target[0], variables.get(target[0])!!*value); }
            "jnz"->{
                    if(getVar(cut[1])!=0)
                        currentLine += value-1;
                }
            else-> throw Exception("file format wrong");
        } 
        currentLine++;
    }
    multiple;
}