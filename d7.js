var allBags = {};
var raw = document.querySelector("pre").textContent.split("\n").filter(a=>a!="");
var data = raw.map(item=>parse(item.slice(0, -1)));
function parse(str){
   var data0 = str.split(" contain ");
   var count = parseInt(data0[1]);
   if(isNaN(count)){
      allBags[data0[0]] = [];
      return;
   }
   else {
      allBags[data0[0]] =
         data0[1].split(", ").map(b=>{
            var count = parseInt(b);
            return {
               count: count,
               item: b.substring(b.indexOf(" ")+1)+(count>1?"":"s")
            }
         });
   }
}
//------------------------ 1
var set = new Set();
function getParentBag(bag) {
   for(let bagKey of Object.keys(allBags))
   {
      let value = allBags[bagKey];
      if(value.length == 0) continue;
      let existingBag = value.filter(item=>item.item===bag);
      if(existingBag.length > 0) { getParentBag(bagKey); set.add(bagKey); }
   }
}
getParentBag("shiny gold bags")
console.log(set.size);
console.log(set);

//------------------------ 2
function searchBag(bag) {
   var val = allBags[bag];
   if(!val?.length) return 1;
   var sum = 1;
   for(var v of val){
      sum+= searchBag(v.item)*v.count;
   }
   return sum;
}

console.log(searchBag("shiny gold bags") -1);