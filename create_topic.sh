#!/bin/bash

# Wait until Zookeeper is ready
until kafka-topics.sh --zookeeper zookeeper:2181 --list; do
  sleep 1
done

# Wait until Kafka broker is ready
until kafka-broker-api-versions --bootstrap-server kafka:9092; do
  sleep 1
done

# Create the topic
kafka-topics.sh --create --topic phonebook-topic --bootstrap-server kafka:9092 --partitions 1 --replication-factor 1
