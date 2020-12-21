const data = document.querySelector("pre").textContent
    .split("\n\n")
    .filter(x=>x!="")
    .map(x=>x.split("\n").map(x=>x.split(" ")).flat()
        .reduce((acc, item)=>{
            let sp = item.split(":");
            acc[sp[0]] = sp[1];
            return acc;
        },
        {}
    ));
const required = ["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"];

//1
const usersWithValidKeys = data.filter(user => required.every(r => Object.keys(user).includes(r)));
console.log(usersWithValidKeys.length);

//2
//lol JavaScript doesn't need parsing numbers
const minMax = (value, min, max) => (min <= value) && (value <= max)
const checkHeight = (height) => {
    const size = height.slice(0,-2);
    switch(height.slice(-2)){
        case "cm":
            return minMax(size, 150, 193);
        case "in":
            return minMax(size, 59, 76);
    }
}
const maxHex = parseInt("FFFFFF",16);
const checkHcl = (hair) => minMax(parseInt(hair.substring(1), 16), 0, maxHex);
const eclList = ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"];

let valid = 0;
for(var user of usersWithValidKeys)
{
    if(
        minMax(user.byr, 1920, 2002)
        &&
        minMax(user.iyr, 2010, 2020)
        &&
        minMax(user.eyr, 2020, 2030)
        &&
        checkHeight(user.hgt)
        &&
        checkHcl(user.hcl)
        &&
        eclList.includes(user.ecl)
        &&
        user.pid.length == 9
        &&
        !isNaN(user.pid)
    )
    {
        valid++;
    }
}
