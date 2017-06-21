/// <binding Clean='default' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
var bower = require('gulp-bower-files');
var cleanCSS = require('gulp-clean-css');
var rename = require("gulp-rename");
var less = require('gulp-less');

var paths = {
    webroot: "./wwwroot/"
};
paths.jqueryjs = paths.webroot + "lib/jquery/dist/jquery.js";
paths.bootstrapjs = paths.webroot + "lib/bootstrap/dist/js/bootstrap.js";
paths.sitejs = paths.webroot + "js/site.js";
paths.accountjs = paths.webroot + "js/account.js";
paths.postjs = paths.webroot + "js/post.js";

paths.siteless = paths.webroot + "css/site.less";

paths.bootstrapcss = paths.webroot + "lib/bootstrap/dist/css/bootstrap.css";
paths.fontcss = paths.webroot + "lib/bootstrap/dist/css/font-awesome.css";
paths.sitecss = paths.webroot + "css/site.css";
paths.accountcss = paths.webroot + "css/account.css";

gulp.task("minify-js", function () {
    gulp.src([paths.jqueryjs, paths.bootstrapjs, paths.sitejs])
        .pipe(uglify())
        .pipe(concat("site.min.js"))
        .pipe(gulp.dest(paths.webroot + "lib/site/js"));

    gulp.src([paths.accountjs])
        .pipe(uglify())
        .pipe(concat("account.min.js"))
        .pipe(gulp.dest(paths.webroot + "lib/site/js"));

    gulp.src([paths.postjs])
        .pipe(uglify())
        .pipe(concat("post.min.js"))
        .pipe(gulp.dest(paths.webroot + "lib/site/js"));
});

gulp.task('minify-less', function () {
    return gulp.src(paths.siteless)
        .pipe(less())
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(gulp.dest(paths.webroot + "css"))
});

gulp.task('minify-css', function () {
    gulp.src([paths.sitecss])
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(concat("site.min.css"))
        .pipe(gulp.dest(paths.webroot + 'lib/site/css'));
    gulp.src([paths.accountcss])
        .pipe(cleanCSS({ compatibility: 'ie8' }))
        .pipe(concat("account.min.css"))
        .pipe(gulp.dest(paths.webroot + 'lib/site/css'));
});

gulp.task('default', ["minify-less", "minify-js", "minify-css"]);