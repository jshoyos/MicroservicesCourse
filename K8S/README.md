# 1. K8S Commands to get started
- `kubectl apply -f platforms-depl.yaml`
- `kubectl apply -f commands-depl.yaml`
- `kubectl apply -f platforms-np-srv.yaml`
- `kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.3.1/deploy/static/provider/aws/deploy.yaml`
- `kubectl apply -f ingress-srv.yaml`

# 2. K8S commands to see pods, deployments, services
- `kubectl get deployments`
```
NAME             READY   UP-TO-DATE   AVAILABLE   AGE 
commands-depl    1/1     1            1           84m  
mssql-depl       1/1     1            1           5m21s
platforms-depl   1/1     1            1           171m
```
- `kubectl get services`
```
NAME                           TYPE          PORT(S)    AGE
commandservice-clusterip-srv   ClusterIP     80/TCP     91m
kubernetes                     ClusterIP     443/TCP    23d
mssql-clusterip-srv            ClusterIP    1433/TCP    12m
mssql-loadbalancer             LoadBalancer 1433:32634/TCP   12m
platformnpservice-srv          NodePort   80:30874/TCP  18d
platforms-clusterip-srv        ClusterIP  80/TCP        94m
```
- `kubectl get pods`
```
NAME                         READY   STATUS   RESTARTS  AGE
commands-depl-5f967449cc-ncrcc    1/1     Running   0          94m
mssql-depl-5cd6d7d486-nznj8       1/1     Running   1          15m
platforms-depl-5494d9b5ff-28jqm   1/1     Running   0          82m
```
- `kubectl get deployments -n ingress-nginx`
```
NAME                   READY   UP-TO-DATE   AVAILABLE   AGE
ingress-nginx-controller   1/1     1            1       71m
```
# 3. K8S create secrets
- `Kubectl create secret generic <secretName> --from-literal=<keyName>=<secretValue>`