Windows Service Data Generator and Publisher
Overview
This project is a Windows Service that generates simulated data, aggregates it, and sends it to a message broker (such as RabbitMQ, Kafka, or Redis Pub/Sub) every second. This service is designed to handle high data throughput and can generate and send over 5,000 data points per second.

Prerequisites
.NET 9
RabbitMQ / Kafka / Redis Pub/Sub (use a locally installed version for testing)
