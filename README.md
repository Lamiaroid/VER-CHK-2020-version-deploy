# VER-CHK
# ***** REQUIREMENTS *****
MongoDB v4.2.3 or higher;<br/>
Visual Studio;<br/>
Internet Connection;<br/>

# ***** HOW TO USE *****
Create (if doesn't exist) folder C:/data/db;<br/>
Start MondoDB Server;<br/>
Open project in Visual Studio and Start;<br/>

# ***** REMARKS *****
If you want to enable restore password by email function, you should change the following lines in ArticleService.cs to your login and password and enable "Less secure apps to access Gmail" option in your google account.
    ```c#
    // take your login
    const string SenderLogin = "test@gmai.com";
    ```

    ```c#
    // take your password
    const string SenderPassword = "admin";
    ```

# ***** ABILITIES *****
You can: <br/>
-create new articles, edit and delete them (if you have already logged in);<br/>
-create new profiles;<br/>
-search articles by filters (particle title occurrency, author name and category);<br/>
-view any article simply by clicking on it in the articles list;<br/>
-post comments (if you have already logged in);<br/>
-receive forgotten passwords by email (if you have already set sender password and login);<br/>
<br/>
You cannot:<br/>
-create accounts with the same names or emails;<br/>
-create accounts with empty names, emails or passwords;<br/>
-post empty comments or comments larger than 200 symbols;<br/>
-create articles with empty titles, content or categories;<br/>
-create articles with the same titles or content larger than 2000 symbols;<br/>
-change articles data on empty content or categories;<br/>
-receive password if your email is incorrect or empty;<br/>
