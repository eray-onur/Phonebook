# Phonebook Application

## Introduction

This is a simple implementation of two microservices that make up a phonebook directory together which communicates through a Kafka container with one single topic and a partition. Orchestrated entirely by Docker Compose. 

### Directory

The service that actually manages the modification and query of adding new people and contact information.

### Report

Service that generates reports based on the aggregated data from the directory service.

## Setup Instructions

Type "docker-compose up" command in the project's root directory. Works out of the box.

However, if you'd like to work with it in your local development environment, simply deactivate the api's in your docker environment follow the changes that will be mentioned below:

1. Change `app.Run("http://*:8080");` to `app.Run();` in your Report API's Program.cs.

2. Change `app.Run("http://*:8081");` to `app.Run();` in your Directory API's Program.cs.

3. Change the kafka bootstrap servers parameter in both services' appsettings.json from `"BootstrapServers": "kafka:9092"` to `"BootstrapServers": "localhost:9092"`.

4. Change host part of both api's db connection from their relevant docker container service names (directorydb, reportdb) to localhost.

Should be good to go after this!

### Usage Instructions

If you'd like to use the API without preparing http requests for it by yourself, you can use the swagger interface to do it instead.

For Directory API, which contains endpoints that can used to generate person and contact info, you may visit (http://localhost:5000/swagger/index.html) when the server container is up.

For Report API, which contains endpoints that can be used to display, list or generate new reports, you may visit (http://localhost:5001/swagger/index.html) when the server container is up. Coincidentally, it relies on the directory API for the person and contact information.


