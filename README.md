# "Band Tracker" An exercise in object-oriented csharp

#### _Band Tracker_, 03.03.2017

### By _Sam Kirsch_

## Description

#### A website built as an exercise in sql database creation and interaction. Users may input bands and venues, and link the two together into bands that have played at venues and venues that have hosted bands.

## Specifications

* Users can add bands
* Users can add venues
* Users can link bands and venues
* Users can update venues
* Users can delete venues
* Users can search by keyword through bands and venues

#### Stretch Goals

* Spruce up the site
* Implement additional CRUD functionality for Bands
* Create better user flow (more accessibility to button, easier links)

## Setup

* >Clone this repository
* >Enter these commands into a local SQL database terminal editor:
*  CREATE DATABASE band_tracker;
*  USE band_tracker;
*  CREATE TABLE bands (id INT IDENTITY(1,1), name VARCHAR(255));
*  CREATE TABLE venues (id INT IDENTITY(1,1), name VARCHAR(255));
*  CREATE TABLE bands_venues (id INT IDENTITY(1,1), band_id INT, venue_id INT);

To create the test database, repeat the above steps, but CREATE DATABASE band_tracker_test, instead

* >Initialize a local kestrel server
* >Navigate to http://localhost:5004/

### Technologies Used

* HTML
* msSql
* CSS with bootstrap and materialize
* CSharp using Nancy, Razor, and Xunit

[gh-pages link for this project](https://denalisk.github.io/band_tracker)

##### Copyright (c) 2017 Sam Kirsch.

##### Licensed under the MIT license.
