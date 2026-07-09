## UI Requirement

Create a simple HTML UI that loads a Course Gallery from a Web API. 

## Course Gallery UI implementation

To serve this file, you must have a `wwwroot` folder and call `app.UseStaticFiles()` in your `Program.cs`.

### `wwwroot/course-gallery.html`

```html
<!DOCTYPE html>
<html>
<head>
    <title>Course Gallery</title>

    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f6f8;
            margin: 0;
            padding: 30px;
        }

        h1 {
            text-align: center;
            color: #222;
        }

        .subtitle {
            text-align: center;
            color: #666;
            margin-bottom: 30px;
        }

        .course-container {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
            gap: 25px;
            max-width: 1200px;
            margin: auto;
        }

        .course-card {
            background-color: white;
            border-radius: 14px;
            overflow: hidden;
            box-shadow: 0 4px 14px rgba(0, 0, 0, 0.12);
        }

            .course-card img {
                width: 100%;
                height: 170px;
                object-fit: cover;
            }

        .course-content {
            padding: 18px;
        }

        .course-title {
            font-size: 20px;
            font-weight: bold;
            color: #222;
            margin-bottom: 8px;
        }

        .course-info {
            color: #555;
            margin: 6px 0;
            font-size: 14px;
        }

        .fee {
            font-size: 18px;
            font-weight: bold;
            color: #0a7cff;
            margin-top: 12px;
        }

        .enroll-button {
            width: 100%;
            margin-top: 15px;
            padding: 12px;
            border: none;
            border-radius: 8px;
            background-color: #0a7cff;
            color: white;
            font-size: 15px;
            cursor: pointer;
        }

            .enroll-button:hover {
                background-color: #005fcc;
            }
    </style>
</head>

<body>

    <h1>Training Course Gallery</h1>
    <p class="subtitle">Courses loaded from ASP.NET Core Web API</p>

    <div id="courseContainer" class="course-container"></div>

    <script>
        async function loadCourses() {
            const response = await fetch('/api/course-tiles', {
                headers: {
                    'X-Client-Name': 'CourseGalleryUI'
                }
            });

            if (!response.ok) {
                const errorText = await response.text();
                alert("API failed: " + errorText);
                return;
            }

            const courses = await response.json();

            const container = document.getElementById('courseContainer');

            courses.forEach(course => {
                const card = document.createElement('div');
                card.className = 'course-card';

                card.innerHTML = `
                    <img src="${course.imageUrl}" alt="${course.title}">
                    <div class="course-content">
                        <div class="course-title">${course.title}</div>
                        <div class="course-info">Category: ${course.category}</div>
                        <div class="course-info">Duration: ${course.durationInDays} Days</div>
                        <div class="course-info">Level: ${course.level}</div>
                        <div class="fee">₹${course.fee}</div>
                        <button class="enroll-button" onclick="enroll(${course.courseId})">
                            Enroll Now
                        </button>
                    </div>
                `;

                container.appendChild(card);
            });
        }

        function enroll(courseId) {
            alert("Enrollment clicked for Course Id: " + courseId);
        }

        loadCourses();
    </script>
</body>
</html>
```
