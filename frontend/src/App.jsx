import React, { useEffect, useMemo, useState } from 'react';
import { Activity, ArrowDownToLine, Boxes, CheckCircle2, Clock3, Factory, History, Package, RefreshCw, Wifi, WifiOff } from 'lucide-react';

const API = (import.meta.env.VITE_API_URL || 'http://localhost:5000').replace(/\/$/, '');
const pad = n => String(n).padStart(2, '0');
const localDate = () => { const d = new Date(); return `${d.getFullYear()}-${pad(d.getMonth()+1)}-${pad(d.getDate())}`; };
const seconds = n => `${Number(n || 0).toFixed(1)} s`;

export default function App() {
  const [orders, setOrders] = useState([]), [apiOnline, setApiOnline] = useState(false), [activeTab, setActiveTab] = useState('registro');
  const [email, setEmail] = useState(''), [orderId, setOrderId] = useState(''), [date, setDate] = useState(localDate()), [quantity, setQuantity] = useState(''), [material, setMaterial] = useState('');
  const [started, setStarted] = useState(0), [elapsed, setElapsed] = useState(0), [notice, setNotice] = useState(null), [submitting, setSubmitting] = useState(false);
  const [historyEmail, setHistoryEmail] = useState(''), [history, setHistory] = useState([]), [historyState, setHistoryState] = useState('idle');
  const selected = useMemo(() => orders.find(x => x.order === orderId), [orders, orderId]);

  async function loadOrders() {
    try { const r = await fetch(`${API}/api/orders/GetOrders`); if (!r.ok) throw new Error(); const d = await r.json(); setOrders(d.orders || []); setApiOnline(true); }
    catch { setApiOnline(false); }
  }
  useEffect(() => { loadOrders(); const id = setInterval(loadOrders, 30000); return () => clearInterval(id); }, []);
  useEffect(() => { if (!started) return; const id = setInterval(() => setElapsed((Date.now()-started)/1000), 100); return () => clearInterval(id); }, [started]);
  function selectOrder(value) { setOrderId(value); setMaterial(''); setNotice(null); if (value) { const now = Date.now(); setStarted(now); setElapsed(0); } else { setStarted(0); setElapsed(0); } }
  async function submit(e) {
    e.preventDefault(); setNotice(null);
    if (!selected || !email.trim() || !date || !quantity || !material) { setNotice({ok:false,text:'Preencha todos os campos obrigatórios.'}); return; }
    setSubmitting(true);
    try {
      const r = await fetch(`${API}/api/orders/SetProduction`, { method:'POST', headers:{'Content-Type':'application/json'}, body:JSON.stringify({email:email.trim(),order:orderId,productionDate:date,productionTime:new Date().toTimeString().slice(0,8),quantity:Number(quantity),materialCode:material,cycleTime:Number(elapsed.toFixed(2))}) });
      const result = await r.json(); setApiOnline(true); setNotice({ok:r.ok,text:result.description});
      if (r.ok) { setOrderId(''); setMaterial(''); setQuantity(''); setStarted(0); setElapsed(0); }
    } catch { setApiOnline(false); setNotice({ok:false,text:'Não foi possível conectar à API. Confira se o back-end está em execução.'}); }
    finally { setSubmitting(false); }
  }
  async function searchHistory(e) {
    e?.preventDefault(); if (!historyEmail.trim()) return; setHistoryState('loading');
    try { const r = await fetch(`${API}/api/orders/GetProduction?email=${encodeURIComponent(historyEmail.trim())}`); if (!r.ok) throw new Error(); const d = await r.json(); setHistory(d.productions || []); setHistoryState('ready'); setApiOnline(true); }
    catch { setHistoryState('error'); setApiOnline(false); }
  }
  const remaining = selected ? Math.max(0, selected.cycleTime-elapsed) : 0;

  return <div className="app-shell">
    <header className="topbar"><div className="brand"><div className="brand-icon"><Factory size={21}/></div><div><b>SEQUOR<span> / </span>PRODUCTION</b><small>APONTAMENTO INDUSTRIAL</small></div></div><div className="top-meta"><div className="system-label"><span className="live-dot"/> SISTEMA OPERACIONAL</div><div className={`connection ${apiOnline?'online':'offline'}`}>{apiOnline?<Wifi size={15}/>:<WifiOff size={15}/>} API {apiOnline?'CONECTADA':'DESCONECTADA'}</div></div></header>
    <main className="container"><div className="page-heading"><div><div className="eyebrow">CENTRO DE CONTROLE <span>•</span> PRODUÇÃO</div><h1>Apontamento de produção</h1><p>Registre o volume produzido e acompanhe sua eficiência por ordem.</p></div><div className="shift-card"><span className="shift-mark"><Activity size={18}/></span><div><small>AMBIENTE</small><b>OPERAÇÃO PADRÃO</b></div></div></div>
      <nav className="tabs"><button className={activeTab==='registro'?'active':''} onClick={()=>setActiveTab('registro')}><ArrowDownToLine size={16}/> Novo apontamento</button><button className={activeTab==='historico'?'active':''} onClick={()=>setActiveTab('historico')}><History size={16}/> Histórico de produção</button></nav>
      {activeTab==='registro' ? <section className="layout"><div className="form-card"><div className="section-title"><div className="step">01</div><div><h2>Dados do apontamento</h2><p>Informe os dados da operação realizada.</p></div></div>
        <form onSubmit={submit}><label>E-mail do operador<span className="required">*</span><input type="email" placeholder="operador@empresa.com.br" value={email} onChange={e=>setEmail(e.target.value)} required/></label>
          <label>Ordem de produção<span className="required">*</span><select value={orderId} onChange={e=>selectOrder(e.target.value)} required><option value="">Selecione uma ordem</option>{orders.map(o=><option key={o.order} value={o.order}>OP {o.order} · {o.productDescription}</option>)}</select></label>
          <div className="field-row"><label>Data do apontamento<span className="required">*</span><input type="date" value={date} onChange={e=>setDate(e.target.value)} required/></label><label>Quantidade produzida<span className="required">*</span><div className="input-unit"><input type="number" min="0.01" step="0.01" placeholder="0,00" value={quantity} onChange={e=>setQuantity(e.target.value)} required/><span>un.</span></div></label></div>
          <label>Material utilizado<span className="required">*</span><select value={material} onChange={e=>setMaterial(e.target.value)} disabled={!selected} required><option value="">{selected?'Selecione o material':'Selecione uma ordem primeiro'}</option>{selected?.materials.map(m=><option key={m.materialCode} value={m.materialCode}>{m.materialCode.toUpperCase()} · {m.materialDescription}</option>)}</select></label>
          <div className="cycle-box"><div className="cycle-label"><span className="cycle-icon"><Clock3 size={17}/></span><div><b>Tempo de ciclo</b><small>Calculado automaticamente</small></div></div><strong>{seconds(elapsed)}</strong></div>
          {selected && remaining>0 && <div className="wait-note"><Clock3 size={15}/> Tempo mínimo da ordem: {seconds(selected.cycleTime)}. Aguarde mais {seconds(remaining)}.</div>}
          {notice && <div className={`notice ${notice.ok?'success':'error'}`}>{notice.ok&&<CheckCircle2 size={17}/>}<span>{notice.text}</span></div>}
          <button className="submit" disabled={submitting||!selected||elapsed<selected.cycleTime}>{submitting?'Enviando apontamento…':<><ArrowDownToLine size={17}/> Enviar apontamento</>}</button>
        </form>
      </div><aside className="side-column"><div className="product-card"><div className="card-overline"><span>VISUALIZAÇÃO</span><span className="badge">PRODUTO</span></div>{selected?<><img className="product-image" src={`${API}${selected.image}`} alt={selected.productDescription}/><div className="product-heading"><div><small>PRODUTO {selected.productCode.toUpperCase()}</small><h3>{selected.productDescription}</h3></div><div className="product-symbol"><Package size={19}/></div></div><div className="product-stats"><div><small>QUANTIDADE DA OP</small><b>{selected.quantity.toLocaleString('pt-BR')} <i>un.</i></b></div><div><small>CICLO CADASTRADO</small><b>{seconds(selected.cycleTime)}</b></div></div><div className="materials"><small>MATERIAIS PERMITIDOS</small><div>{selected.materials.map(m=><span key={m.materialCode}>{m.materialCode.toUpperCase()} · {m.materialDescription.split(' - ')[0]}</span>)}</div></div></>:<div className="empty-product"><div><Boxes size={26}/></div><b>Nenhuma ordem selecionada</b><span>Selecione uma ordem para visualizar<br/>os detalhes do produto.</span></div>}</div>
        <div className="tip-card"><div className="tip-icon"><Clock3 size={18}/></div><div><b>Contagem de ciclo ativa</b><p>O tempo começa a contar assim que uma ordem é selecionada.</p></div></div></aside></section> : <section className="history-card"><div className="section-title"><div className="step"><History size={17}/></div><div><h2>Histórico de produção</h2><p>Consulte os apontamentos registrados por operador.</p></div></div><form className="history-search" onSubmit={searchHistory}><label>E-mail do operador<input type="email" placeholder="operador@empresa.com.br" value={historyEmail} onChange={e=>setHistoryEmail(e.target.value)} required/></label><button className="submit" type="submit" disabled={historyState==='loading'}><RefreshCw size={16}/> {historyState==='loading'?'Consultando…':'Consultar histórico'}</button></form>{historyState==='error'?<div className="empty-history">Falha ao consultar a API. Verifique a conexão e tente novamente.</div>:historyState==='ready'?(history.length===0?<div className="empty-history">Nenhum apontamento encontrado para este e-mail.</div>:<div className="table-wrap"><table><thead><tr><th>ORDEM</th><th>DATA E HORA</th><th>QUANTIDADE</th><th>MATERIAL</th><th>TEMPO DE CICLO</th></tr></thead><tbody>{history.map((x,i)=><tr key={`${x.order}-${x.date}-${i}`}><td><b>OP {x.order}</b></td><td>{new Date(x.date).toLocaleString('pt-BR')}</td><td>{x.quantity.toLocaleString('pt-BR')} un.</td><td>{x.materialCode.toUpperCase()}</td><td>{seconds(x.cycleTime)}</td></tr>)}</tbody></table></div>):<div className="empty-history">Informe um e-mail para consultar os registros de produção.</div>}</section>}
      <footer><span>SEQUOR PRODUCTION <i>•</i> SISTEMA DE APONTAMENTO</span><span>Versão 1.0.0</span></footer>
    </main>
  </div>;
}
