sudo docker network create homeautomationnetwork
sudo docker run -d --name redis --restart unless-stopped -v /redis-data:/data --network homeautomationnetwork redis redis-server --save 60 1
sudo docker run -d -p 80:80 --device /dev/gpiomem --restart unless-stopped --network homeautomationnetwork 192.168.0.149:9876/homeautomationserver:20211017140421