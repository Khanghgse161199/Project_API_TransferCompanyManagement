async function SignUp(){
    var username = document.getElementById("inputUserName").value;
    var password = document.getElementById("inputPassWord").value;
    var fullname = document.getElementById("inputFullName").value;
    var address = document.getElementById("inputAddress").value;
    var phone = document.getElementById("inputPhone").value;
    var email = document.getElementById("inputEmail").value;
    var citizenId = document.getElementById("inputCitizenId").value;

    var createEmployeeModel = {
        Username : username,
        Password : password,
        FullName : fullname,
        Address : address,
        Phone : phone,
        Email : email,
        CitizenId : citizenId,
    }

    if(createEmployeeModel != null){
        var url = "https://localhost:7031/api/Employee/CreateEmployee";
        var repone = await fetch(url,{
            method : "Post",
            headers:{
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(createEmployeeModel)           
        });
        if(repone.ok){
            alert("Success")        
        }else alert("Error");
    }
} 