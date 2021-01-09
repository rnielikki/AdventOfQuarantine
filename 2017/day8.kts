import java.io.File;

val variables = HashMap<String, Int>();
var max = Int.MIN_VALUE;
File("day8.txt").forEachLine{
    fun getVar(value:String):Int{
        if(!variables.contains(value))
        {
            variables.put(value,0);
            return 0;
        }
        else{
            return variables[value]!!;
        }
    }
    fun checkCondition(variable:String, operator:String, comaprison:Int):Boolean{
        val value = getVar(variable);
        return when(operator){
            ">" -> value > comaprison
            "<" -> value < comaprison
            ">=" -> value >= comaprison
            "<=" -> value <= comaprison
            "!=" -> value != comaprison
            "==" -> value == comaprison
            else -> throw Exception("file format wrong");
        }
    }
    fun getMax (one:Int, two:Int):Int{
        if(one < two) return two;
        else return one;
    }
    val cut = it.split(' ');
    if(!checkCondition(cut[4], cut[5], cut[6].toIntOrNull()!!)){
        return@forEachLine
    }
    val target = cut[0];
    val value = cut[2].toIntOrNull()!!;
    when(cut[1])
    {
        "inc"->
            variables.set(target, getVar(target)+value);
        "dec"->
            variables.set(target, getVar(target)-value);
        else-> throw Exception("file format wrong");
    }
    max = getMax(variables[target]!!, max)
}
println(variables.maxByOrNull{ it.value }!!.value);
println(max);