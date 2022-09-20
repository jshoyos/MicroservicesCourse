kubectl apply -f .\local-pvc.yaml
kubectl apply -f .\mssql-plat-depl.yaml
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.3.1/deploy/static/provider/aws/deploy.yaml
kubectl apply -f .\platforms-depl.yaml
kubectl apply -f .\commands-depl.yaml
kubectl apply -f .\platforms-np-srv.yaml
kubectl apply -f .\ingress-srv.yaml
kubectl apply -f .\rabbitmq-depl.yaml