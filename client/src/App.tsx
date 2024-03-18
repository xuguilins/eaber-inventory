import AppLayout from "@/components/Layout";
import WatchLayout from "./WatchLayout";
import KeepAlive, { AliveScope } from "react-activation";
import { useLocation, useNavigate } from "react-router-dom";
import HubClient from "./utils/hubCenter";

function App() {
  return (
    <AppLayout />
  );
}

export default App;
