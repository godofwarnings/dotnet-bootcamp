# 04 - Load Balancer vs. Traffic Manager
 
Two different layers of "spread the load", often confused:
 
| | **Load Balancer** | **Traffic Manager** |
| --- | --- | --- |
| **Layer** | Across servers/VMs **within a region** | Across **regions/endpoints** |
| **Mechanism** | Network-level (Azure Load Balancer = L4/TCP-UDP; Application Gateway = L7/HTTP) | **DNS-based** — returns the address of the best endpoint |
| **Answers** | "Which healthy server handles this request?" | "Which region/data-center should this user reach?" |
| **Typical use** | Scale-out & HA within a region | Global failover, geo-routing, latency routing |
 
```mermaid
flowchart TD
    User(["User request"])
    TM{"Traffic Manager<br/>(DNS — pick a region)"}
    subgraph R1["Region: East US"]
        LB1["Load Balancer"]
        S1["Server A"]
        S2["Server B"]
        LB1 --> S1
        LB1 --> S2
    end
    subgraph R2["Region: West Europe"]
        LB2["Load Balancer"]
        S3["Server C"]
        S4["Server D"]
        LB2 --> S3
        LB2 --> S4
    end
 
    User --> TM
    TM -->|nearest / healthy region| LB1
    TM -->|failover| LB2
```
 
> [!NOTE] The key distinction
> **Traffic Manager works at the DNS level**, so it decides *before* the connection is made and cannot see individual requests. A **Load Balancer** sits in the actual request path within a region and balances per-connection. They compose: Traffic Manager picks the region, the region's Load Balancer picks the server. (This is the infrastructure realization of the **Disaster Recovery** targets in **Part 1 → Phase 6**.)
