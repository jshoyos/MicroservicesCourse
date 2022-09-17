kubectl delete --all deployments
kubectl delete deployment -n ingress-nginx ingress-nginx-controller
kubectl delete service commandservice-clusterip-srv mssql-clusterip-srv mssql-loadbalancer platformnpservice-srv platforms-clusterip-srv
kubectl delete service -n ingress-nginx ingress-nginx-controller ingress-nginx-controller-admission