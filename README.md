# Watch-Store WebSite
![Presentation Project](/UML/MainPage.png)

 ASP .Net Web Application Development - Watch Store WebSite
 
---
# Back-End
## Implements
C# | Javascript | jQuery | Entity Framework | .NET | MVC

### Models & Controllers
Customers | Brokers | Deals | Properties 

### SQL

#### GROUP BY

```scala
 public ActionResult Group()
        {
            var group = (from bo in db.Properties
                         group bo by bo.PropertyType into j
                         select new Group<string, Property> { Key = j.Key, Values = j });
            return View(group.ToList());
        }
```

#### JOIN

```scala
 public ActionResult Join()
        {
            string CustomerName = TempData["name"].ToString();
            var Delas = (from bo in db.Customers
                         join lo in db.Deals
                         on bo.CustomerID equals lo.CustomerID
                         where bo.CustomerFirstName.StartsWith(CustomerName)
                         select lo);
            return View();   
       }
```


### jQuery & Ajax
```scala
<script>
        function cycleImages() {
            var $active = $('#cycler .active');
            var $next = ($active.next().length > 0) ? $active.next() : $('#cycler img:first');
            $next.css('z-index', 2);//move the next image up the pile
            $active.fadeOut(1500, function () {//fade out the top image
                $active.css('z-index', 1).show().removeClass('active');//reset the z-index and unhide the image
                $next.css('z-index', 3).addClass('active');//make the next image the top one
            });
        }

        $(document).ready(function () {
            // run every 7s
            setInterval('cycleImages()', 3000);
        })
    </script>
```

```scala
    <script>
        $(document).ready(function () {
            $('#photo').fadeToggle(1000).fadeToggle(1300);
        });
    </script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="main.js"></script>
```

---
# Front-End
## Implements
HTML\CSS | Bootsrap  

### HTML5
#### VIDEO
```scala
<div class="vid">
        <iframe frameborder="1" scrolling="yes" marginheight="0" marginwidth="0" width="1135" height="500" type="text/html" src="https://www.youtube.com/embed/rzSUKaz1lHY?autoplay=1&fs=1&iv_load_policy=3&showinfo=0&rel=0&cc_load_policy=0&start=0&end=0&vq=hd1080"></iframe>
    </div>
```
#### CANVAS
```scala
function draw() {
  var canvas = document.getElementById('canvas');
  if (canvas.getContext) {
    var ctx = canvas.getContext('2d');

    // Filled triangle
    ctx.beginPath();
    ctx.moveTo(25, 25);
    ctx.lineTo(105, 25);
    ctx.lineTo(25, 105);
    ctx.fill();

    // Stroked triangle
    ctx.beginPath();
    ctx.moveTo(125, 125);
    ctx.lineTo(125, 45);
    ctx.lineTo(45, 125);
    ctx.closePath();
    ctx.stroke();
  }
}
```

```scala
 <script>
        window.onload = function () {
                    var c = document.getElementById("myCanvas");
                    var ctx = c.getContext("2d");
                    var img = document.getElementById("admin");
                    ctx.drawImage(img, 10, 10,150,150);
                }
    </script>
```
### CSS3
Text-shadow | Transition | Multiple-columns | Font-face | Border-radius


### Accsess
Separation using TempData

```scala
 <ul class="nav navbar-nav navbar-right">
                    @if (TempData["Role"] != null)
                    {
                        <li>@Html.ActionLink("Log out", "Logout", "Home")</li>
                        if (TempData["Role"].ToString().Equals("Admin"))
                        {
                            <li style="margin-top:15px; margin-right:5px;"> @TempData["name"].ToString() </li><img src="~/Content/Resources/Team/Gal.jpeg" style="margin-top:1rem; width:30px; height:30px;" class="profile-image img-circle"> 
                        }
                        else
                        {
                            <li style="margin-top:15px; margin-right:5px;"> @TempData["name"].ToString() </li>
                        }
                        TempData.Keep();
                    }
                    else
                    {
                        <li>@Html.ActionLink("Login", "Login", "Home")</li>
                    }
</ul>
```



### APIs

#### Google Maps

![Presentation Project](/UML/GoogleMaps.png)

```scala
  <div class="mapouter">
        <div class="gmap_canvas"><iframe width="1140" height="500" id="gmap_canvas" src="https://maps.google.com/maps?q=%D7%A8%D7%95%D7%98%D7%A9%D7%99%D7%9C%D7%93%2032%20%D7%91%D7%AA%20%D7%99%D7%9D&t=k&z=15&ie=UTF8&iwloc=&output=embed" frameborder="1" scrolling="yes" marginheight="10" marginwidth="0"></iframe></div>
  </div>
```
#### DB Connected Map
![Presentation Project](/UML/Gallery.png)


#### Facebook

![Presentation Project](/UML/Facebook.png)
```scala
  <div class="facebook">
        <h4>Like US! Recommend US! Share US!</h4>
        <div class="fb-like" data-href="https://www.facebook.com/Fadlon-Real-Estate-124652649071990/" data-width="" data-layout="button_count" data-action="like" data-size="large" data-share="false"></div> <div class="fb-like" data-href="https://www.facebook.com/Fadlon-Real-Estate-124652649071990/" data-width="200" data-layout="button_count" data-action="recommend" data-size="large" data-share="true"></div>
    </div>

    <div id="fb-root"></div>
    <script async defer crossorigin="anonymous" src="https://connect.facebook.net/en_US/sdk.js#xfbml=1&version=v7.0"></script>

```

![Presentation Project](/UML/Facebook_share.png)
```scala
 <div id="fb-root"></div>
    <script src="https://connect.facebook.net/en_US/sdk.js" nonce="kvhXltik"></script>

    <script nonce="kvhXltik">
            document.getElementById("submitsearch").onclick = function () { myFunction() };
            function myFunction() {
                FB.init({
                appId: 2382091785433635,
                status: true,
                xfbml: true,
                version: 'v2.9'
            });
            FB.AppEvents.logPageView();
            FB.ui({
                method: 'share',
                href: 'https://www.facebook.com/permalink.php?story_fbid=124657625738159&id=124652649071990',
            }, function (response) { });
            }
    </script>
```

### Web Service

#### Weather

![Presentation Project](/UML/Weather_api.png)
```scala
 <a class="weatherwidget-io" href="https://forecast7.com/en/32d0134d75/bat-yam/" data-label_1="BAT YAM" data-label_2="WEATHER" data-font="Times New Roman" data-icons="Climacons Animated" data-theme="weather_one">BAT YAM WEATHER</a>
    <script>!function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = 'https://weatherwidget.io/js/widget.min.js'; fjs.parentNode.insertBefore(js, fjs); } }(document, 'script', 'weatherwidget-io-js');</script>

```

### Statistics
```scala
 <Script>
 Highcharts.setOptions(Highcharts.theme);

            Highcharts.chart('container2', {
                data: {
                    table: 'datatable2'
                },
                colors: ['#2b908f', '#90ee7e', '#f45b5b', '#7798BF', '#aaeeee', '#ff0066',
                    '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
                chart: {
                    type: 'pie'
                },

                title: {
                    text: 'Deals Statistics'
                },
                subtitle: {
                    text: 'By Property Name'
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            format: '{point.name}: {point.percentage:.1f} %'
                        }
                    }

                },

                tooltip: {
                    headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                    pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
                }
            });
 </script>
```



---
## Build With
* [Microsoft Visual Studio ](https://visualstudio.microsoft.com/) 

## Author
* **[Gal Jacobson](https://www.linkedin.com/in/jacobsongal/)** 
