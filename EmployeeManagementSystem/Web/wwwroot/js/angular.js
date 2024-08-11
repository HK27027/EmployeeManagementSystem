var myApp = angular.module('myApp', []);


myApp.controller('MyController', function ($scope, $http) {
    //port deðiþiklik gösterebilir onun için tek bir yerden kontrol ediliyor 
    $scope.apiLocation = 'https://localhost:7094';
  
    $scope.fnAccount = function () {
        var obj =
        {
            form:
            {
                name: '',
                password: '',
                email: '',
            },
            list: [],
            data: [],
            info: [],
            count: 0,
            danger: false,
            succes: false,
            dangerText: {},
            succesText: {},

            save: function () {
                if (obj.form.email == undefined) {
                    return true;
                }
                var fd = new FormData();

                fd.append('AccountName', obj.form.name);
                fd.append('Email', obj.form.email);
                fd.append('Password', obj.form.password);


                $http({
                    method: 'POST',
                    url: $scope.apiLocation + '/Accounts',
                    data: fd,
                    headers: {
                        'Content-Type': undefined,

                    }
                }).then(function successCallback(response) {
                    obj.data = response.data.item;
                    $scope.alert.action(response);
                    localStorage.removeItem('accountId');
                    localStorage.setItem('accountId', obj.data.accountID);
                    location.href = "/";
                }, function errorCallback(response) {
                    $scope.alert.action(response);
                });

            },
            login: function () {
                var fd = new FormData();
                
                fd.append('Email', obj.form.email);
                fd.append('Password', obj.form.password);


                $http({
                    method: 'POST',
                    url: $scope.apiLocation + '/Accounts/Login',
                    data: fd,
                    headers: {
                        'Content-Type': undefined,

                    }
                }).then(function successCallback(response) {
                    obj.data = response.data.item;
                    $scope.alert.action(response);
                    localStorage.removeItem('accountId');
                    localStorage.setItem('accountId', obj.data.accountID);
                    location.href = "/";
                }, function errorCallback(response) {
                    $scope.alert.action(response);
                });


            },
            loginControl: function () {
                var account = localStorage.getItem('accountId');
                if (account == null) {
                    location.href = "/Login";
                } else {
                    var fd = new FormData();

                    fd.append('Email', obj.form.email);
                    fd.append('Password', obj.form.password);


                    $http({
                        method: 'Get',
                        url: $scope.apiLocation + '/Accounts/'+account,
                        data: fd,
                        headers: {
                            'Content-Type': undefined,

                        }
                    }).then(function successCallback(response) {
                        obj.data = response.data.item;
                        if (obj.data==null) {
                            localStorage.removeItem('accountId');
                            obj.data = [];
                            location.href = '/Login';
                        }
                     
                       
                    }, function errorCallback(response) {
                      
                       
                    });
                }
               


            },
            logOut: function () {
                localStorage.removeItem('accountId');
                obj.data = [];
                location.href = '/Login';
            },


         

        }
        return obj;
    };

    $scope.account = $scope.fnAccount();




    $scope.fnEmployess = function () {
        var obj =
        {
            form:
            {
                name: '',
                password: '',
                email: '',
                position: '',
                search: '',
                departmentId: '',
                departmentName: '',
            },
            filter:
            {
            
                search: '',
                departmentID: '',
            },
            list: [],
            data: [],
            detail: [],
            count: 0,
            danger: false,
            succes: false,
            dangerText: {},
            succesText: {},
            add: function () {
                $('#exampleModaluserAdd').modal('show');
            },
            save: function () {
                var account = localStorage.getItem('accountId');
                var fd = new FormData();

                fd.append('Name', obj.form.name);
                fd.append('Email', obj.form.email);
                fd.append('Position', obj.form.position);
                fd.append('DepartmentID', obj.form.departmentId);
                fd.append('AccountId', account);


                $http({
                    method: 'POST',
                    url: $scope.apiLocation + '/Employees',
                    data: fd,
                    headers: {
                        'Content-Type': undefined,

                    }
                }).then(function successCallback(response) {
                    obj.data = response.data.item;
                    obj.multipleGet();
                    $scope.departments.multipleGet();
                    $scope.departments.departmentDetail();
                    $('#exampleModaluserAdd').modal('hide');
                    $scope.alert.action(response);
                }, function errorCallback(response) {
                 
                    $scope.alert.action(response);
                });

            },

            multipleGet: function () {
                var account = localStorage.getItem('accountId');
                var fd = new FormData();
                fd.append('AccountId', account);
                fd.append('DepartmentID', obj.filter.departmentID);
                fd.append('Search', obj.form.search);
               
                $http({
                    method: 'POST',
                    url: $scope.apiLocation + '/Employees/MultipleGet',
                    data:fd,
                    headers: {
                        'Content-Type': undefined,
                    }
                }).then(function successCallback(response) {

                    if (response.status == 200) {
                        obj.count = response.data.count;
                        obj.list = response.data.item;
                    } else {
                      
                    }
                   
                }, function errorCallback(response) {
                   
                });
            },


            singleGet: function (id) {
                
                $http({
                    method: 'Get',
                    url: $scope.apiLocation + '/Employees/'+id,
                    headers: {
                        'Content-Type': undefined,
                    }
                }).then(function successCallback(response) {

                    if (response.status == 200) {
                        obj.data = [];
                        obj.data.push( response.data.item);
                       // console.log(obj.data);
                        $('#exampleModalupdateUser').modal('show');
                    } else {

                    }
                  
                }, function errorCallback(response) {

                });
            },
            update: function () {
                var account = localStorage.getItem('accountId');
                var fd = new FormData();

                fd.append('Name', obj.data[0].name);
                fd.append('EmployeeID', obj.data[0].employeeID);
                fd.append('Email', obj.data[0].email);
                fd.append('Position', obj.data[0].position);
                fd.append('DepartmentID', obj.data[0].departmentID);
                fd.append('AccountId', account);


                $http({
                    method: 'PUT',
                    url: $scope.apiLocation + '/Employees',
                    data: fd,
                    headers: {
                        'Content-Type': undefined,

                    }
                }).then(function successCallback(response) {
                    obj.data = [];  
                    obj.data.push(response.data.item);
                    obj.multipleGet();
                    obj.employessDetail();
                    $scope.departments.multipleGet();
                    $scope.departments.departmentDetail();
                 
                    $('#exampleModalupdateUser').modal('hide');
                    $scope.alert.action(response);
                }, function errorCallback(response) {
                  
                    $scope.alert.action(response);
                });

            },

            delete: function (id) {

                if (confirm('Silmek istediðine emin misin?')) {
                    $http({
                        method: 'DELETE',
                        url: $scope.apiLocation + '/Employees/' + id ,

                        headers: {
                            'Content-Type': undefined
                        }
                    }).then(function successCallback(response) {
                        if (response.status == 200) {
                            obj.multipleGet();
                            $scope.departments.multipleGet();
                            $scope.departments.departmentDetail();
                            $scope.alert.action(response);
                        } else {

                        }
                    
                    

                    }, function errorCallback(response) {
                      
                        $scope.alert.action(response);

                    });

                }
            },

            employessDetail: function () {
                var id = $scope.getUrlId();
             
                if (id == null) {
                    return true;
                }
                $http({
                    method: 'Get',
                    url: $scope.apiLocation + '/Employees/' + id,
                    headers: {
                        'Content-Type': undefined,
                    }
                }).then(function successCallback(response) {

                    if (response.status == 200) {
                        obj.detail = [];
                        obj.detail.push(response.data.item);
                    } else {

                    }

                }, function errorCallback(response) {

                });
            },
          

        }
        return obj;
    };

    $scope.employess = $scope.fnEmployess();





    $scope.fnDepartments = function () {
        var obj =
        {
            form:
            {
                name: '',
                password: '',
                email: '',
                search: '',
            },
            list: [],
            data: [],
            detail: [],
            count: 0,
            danger: false,
            succes: false,
            dangerText: {},
            succesText: {},
            add: function () {
                $('#exampleModalDepartmentAdd').modal('show');
            },
            save: function () {

              
                var account = localStorage.getItem('accountId');
                var fd = new FormData();

                fd.append('DepartmentName', obj.form.name);
                fd.append('AccountId', account);


                $http({
                    method: 'POST',
                    url: $scope.apiLocation + '/departments',
                    data: fd,
                    headers: {
                        'Content-Type': undefined,

                    }
                }).then(function successCallback(response) {
                    obj.data = response.data.item;
                    obj.multipleGet();
                    $scope.alert.action(response);
                    $('#exampleModalDepartmentAdd').modal('hide');
                }, function errorCallback(response) {
                    $scope.alert.action(response);
                });

            },

            multipleGet: function () {
                var account = localStorage.getItem('accountId');
                var fd = new FormData();
                fd.append('AccountId', account);
                fd.append('Search', obj.form.search);

                $http({
                    method: 'POST',
                    url: $scope.apiLocation + '/Departments/MultipleGet',
                    data: fd,
                    headers: {
                        'Content-Type': undefined,
                    }
                }).then(function successCallback(response) {

                    if (response.status == 200) {
                        obj.count = response.data.count;
                        obj.list = response.data.item;

                        var rnd = Math.floor(Math.random() * 255) + 1;
                        var rnd2 = Math.floor(Math.random() * 255) + 1;
                        var rnd3 = Math.floor(Math.random() * 255) + 1;

                        var ctx = document.getElementById('employeesChart');
                        if (ctx) {
                            // Check if there's an existing chart instance
                            var existingChart = Chart.getChart(ctx);

                            // If there is, destroy it
                            if (existingChart) {
                                existingChart.destroy();
                            }

                            // Create a new chart
                            new Chart(ctx, {
                                type: 'bar',
                                data: {
                                    labels: obj.list.map(function (item) { return item.departmentName; }),
                                    datasets: [
                                        {
                                            label: 'kisi sayisi',
                                            data: obj.list.map(function (item) { return item.empolyeCount; }),
                                            borderWidth: 3,
                                            backgroundColor: `rgba(${rnd}, ${rnd2}, ${rnd3}, 0.2)`,
                                            borderColor: `rgba(${rnd}, ${rnd2}, ${rnd3})`,
                                        }
                                    ]
                                },
                                options: {
                                    responsive: true,
                                    scales: {
                                        y: {
                                            display: true,
                                        }
                                    }
                                }
                            });
                        }
                    } else {

                    }

                }, function errorCallback(response) {

                });
            },


            singleGet: function (id) {
                var a = '';
                $http({
                    method: 'Get',
                    url: $scope.apiLocation + '/Departments/' + id,
                    headers: {
                        'Content-Type': undefined,
                    }
                }).then(function successCallback(response) {

                    if (response.status == 200) {
                        obj.data = [];
                        obj.data.push(response.data.item);
                        //console.log(obj.data);
                        $('#exampleModalupdateDepartment').modal('show');
                    } else {

                    }

                }, function errorCallback(response) {

                });
            },
            update: function () {
                var account = localStorage.getItem('accountId');
                var fd = new FormData();

              
                fd.append('DepartmentID', obj.data[0].departmentID);
                fd.append('DepartmentName', obj.data[0].departmentName);
                fd.append('AccountId', account);


                $http({
                    method: 'PUT',
                    url: $scope.apiLocation + '/Departments',
                    data: fd,
                    headers: {
                        'Content-Type': undefined,

                    }
                }).then(function successCallback(response) {
                    obj.data = [];
                    obj.data.push(response.data.item);
                    obj.multipleGet();
                    $scope.alert.action(response);
                    $('#exampleModalupdateDepartment').modal('hide');
                }, function errorCallback(response) {
                    $scope.alert.action(response);
                });

            },

            delete: function (id) {

                if (confirm('Silmek istediðine emin misin?')) {
                    $http({
                        method: 'DELETE',
                        url: $scope.apiLocation + '/Departments/' + id,

                        headers: {
                            'Content-Type': undefined
                        }
                    }).then(function successCallback(response) {
                        if (response.status == 200) {
                            obj.multipleGet();
                            $scope.employess.multipleGet();
                            $scope.alert.action(response);
                        } else {
                            $scope.alert.action(response);
                        }



                    }, function errorCallback(response) {

                        $scope.alert.action(response);

                    });

                }
            },

            departmentDetail: function () {
                var id = $scope.getUrlId();
                // çalýþan ekleme güncelleme silme iþlemlerinde çaðrýlýyor id herzaman dolu olmaya bilir hata vermemesi için
                if (id==null) {
                    return true;
                }
                $http({
                    method: 'Get',
                    url: $scope.apiLocation + '/Departments/' + id,
                    headers: {
                        'Content-Type': undefined,
                    }
                }).then(function successCallback(response) {

                    if (response.status == 200) {
                        obj.detail = [];
                        obj.detail.push(response.data.item);
                    } else {

                    }

                }, function errorCallback(response) {

                });
            },
        }
        return obj;
    };

    $scope.departments = $scope.fnDepartments();
//fazla sayfa kullanmadýðým için route eklemedim url deki id yi bu þekilde alýyorum 
    $scope.getUrlId = function () {
       
            var path = window.location.pathname;
            var pathParts = path.split('/');
            return pathParts[pathParts.length - 1];
        
    }

    $scope.alert = {
        item: {},
        action: function (response) {
            this.item = response;
            $('#alertModal').modal('show');
        }
    };



});
